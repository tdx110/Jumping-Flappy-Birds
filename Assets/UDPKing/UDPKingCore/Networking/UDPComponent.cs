using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace UDPKing
{
  public class UDPComponent : MonoBehaviour {

	public string  serverIP;

	public int serverPort;
	
	// Type of Peer
	public enum PeerType { Client, Server }; 

	public PeerType peerType;
	
	public enum UDPClientState {DISCONNECTED,CONNECTED,ERROR,SENDING_MESSAGE};
	
	public UDPClientState udpClientState;
	
	/************************************* CLIENT VARIABLES *************************************************/
	private UdpClient udpClient;
	
	private readonly object udpClientLock = new object();
	
	private IPEndPoint endPoint;
	
	private string listenerInput = string.Empty;

	private Thread clientListenner;
	
	/**********************************************************************************************************/
	
	/***************************** SERVER VARIABLES **********************************************************/
	UdpClient udpServer;

  private readonly object udpServerLock = new object();

  private const int bufSize = 8 * 1024;

  private State state = new State();

  private EndPoint epFrom = new IPEndPoint(IPAddress.Any, 0);

  private AsyncCallback recv = null;

  public enum UDPServerState {DISCONNECTED,CONNECTED,ERROR,SENDING_MESSAGE};

  public UDPServerState udpServerState;

  string[] pack;

  private Thread serverListenner;

  private bool stopServer = false;

  public bool serverRunning;

	public class State
	{
		public byte[] buffer = new byte[bufSize];
    }
	 
	/*********************************************************************************************************/
	
	static private readonly char[] Delimiter = new char[] {':'};
	
	string receivedMsg = string.Empty;
	
	private Dictionary<string, List<Action<UDPEvent>>> handlers;
	
	private Queue<UDPEvent> eventQueue;
	
	private object eventQueueLock;
	


	public void Awake()
	{
		handlers = new Dictionary<string, List<Action<UDPEvent>>>();
		eventQueueLock = new object();
		eventQueue = new Queue<UDPEvent>();
		udpClientState = UDPClientState.DISCONNECTED;
		udpServerState = UDPServerState.DISCONNECTED;
		
	}
	
	


	// connect 
	public void connect() {

	    switch (peerType) {

			case PeerType.Client:
			if ( clientListenner != null && clientListenner.IsAlive) {
			  disconnect();
			  while (clientListenner != null && clientListenner.IsAlive) {}
		    }

		   
		     // start  listener thread
		     clientListenner = new Thread(
			 new ThreadStart(OnListeningServer));
		     clientListenner.IsBackground = true;
		     clientListenner.Start();
			break;

			case PeerType.Server:
				Debug.Log("current PeerType Server switch peer Type to Client");
			break;

		} //END_SWITCH
	}
   
		
	public void  OnListeningServer()
	{

		try
		{

			lock ( udpClientLock) {

				
				udpClient = new UdpClient ();
                udpClient.Client.SetSocketOption(
				SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
				udpClient.ExclusiveAddressUse = false;
              

			    try
                {
                 	udpClient.Client.DontFragment = false;
                }
                catch { }

                try
                {
                  const int SIO_UDP_CONNRESET = -1744830452;
                  udpClient.Client.IOControl(SIO_UDP_CONNRESET, new byte[1], null);
                }
                catch { } // Only necessary on Windows

				 try
                {
              
					IPEndPoint localEp = new IPEndPoint(IPAddress.Any, 0);
				    udpClient.Client.Bind(localEp);

                   
                }
                catch (SocketException e)
                {
					Debug.LogError(e.ToString());
              
                }

			
	            udpClientState = UDPClientState.CONNECTED;
				udpClient.BeginReceive (new AsyncCallback (OnWaitPacketsCallback), null);
				
				
			}

		}
		catch
		{
			throw;
		}
	}

	

	public void OnWaitPacketsCallback(IAsyncResult res)
	{
		try{

		lock (udpClientLock) {
 

			byte[] recPacket = udpClient.EndReceive (res, ref endPoint);
			
			if (recPacket != null && recPacket.Length > 0) {
				lock (eventQueueLock) {

				
				
				 if(IsByteStream(recPacket))
				{
				   
					IList<byte[]>  output_pack = new List<byte[]>();
					
				   // Deserialize.
                      using (MemoryStream ms = new MemoryStream(recPacket))
                      {
                        BinaryFormatter formattter = new BinaryFormatter();
						 
                         output_pack =
                             (List<byte[]>)formattter.Deserialize(ms);
						string callbackID = Encoding.UTF8.GetString (output_pack[1]);

					    //enqueue
				         eventQueue.Enqueue(new UDPEvent(callbackID,output_pack[2]));
					     
                        }
				}
				else
				{
					//decode the received bytes vector in string fotmat
					//receivedMsg = "callback_name,param 1,param 2,param n, etc."
					receivedMsg = Encoding.UTF8.GetString (recPacket);

					//separates the items contained in the package using the two points ":" as sifter
					//and it puts them separately in the vector package []
					/*
		            * package[0]= callback_name: e.g.: "PONG"
		            * package[1]= message: e.g.: "pong!!!"
		            * package[2]=  other message for example!
			        */
				var package = receivedMsg.Split (Delimiter);
		            //enqueue
				    eventQueue.Enqueue(new UDPEvent(package [0], receivedMsg));
					receivedMsg = string.Empty;	
					}
				}
			}
			
			udpClient.BeginReceive (new AsyncCallback (OnWaitPacketsCallback), null);
		
		}
		}
		catch(Exception e) 
		{
			Debug.LogError(e.ToString());
			
		
		}
	}


	private void InvokEvent(UDPEvent ev)
	{
		
		if (!handlers.ContainsKey(ev.name)) { return; }
		foreach (Action<UDPEvent> handler in this.handlers[ev.name]) {
			try{
				
				handler(ev);
			} catch(Exception ex){
				
			}
		}
	}

	 public void On(string ev, Action<UDPEvent> callback)
	 {
	    try{

			
		if (!handlers.ContainsKey(ev)) {
			handlers[ev] = new List<Action<UDPEvent>>();
		}
		handlers[ev].Add(callback);
		}
		catch(Exception e) {
			Debug.Log(e.ToString());
		}
	 }

	public void EmitToServer(string callbackID, string _pack)
	{
		
		try{

		 if(udpClientState == UDPClientState.CONNECTED)
		 {
			lock ( udpClientLock) {
			if(udpClient == null)
			{
				
		
					udpClient = new UdpClient ();
				  
                    udpClient.Client.SetSocketOption(
					SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

					udpClient.ExclusiveAddressUse = false;
               

					try
                   {
                   	 udpClient.Client.DontFragment = false;
                   }
                   catch { }

                   try
                   {
                     const int SIO_UDP_CONNRESET = -1744830452;
                     udpClient.Client.IOControl(SIO_UDP_CONNRESET, new byte[1], null);
                   }
                   catch { } // Only necessary on Windows
				
					IPEndPoint localEp = new IPEndPoint(IPAddress.Any, 0);
				    udpClient.Client.Bind(localEp);

			}

			
	        udpClientState = UDPClientState.SENDING_MESSAGE;
			string new_pack = callbackID+":"+_pack;
			byte[] data = Encoding.UTF8.GetBytes (new_pack.ToString ()); //convert to bytes
		
			var endPoint = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);
			udpClient.Send(data, data.Length,endPoint );
		
			udpClientState = UDPClientState.CONNECTED;
			}
		 }
		}
		catch(Exception e) {
			Debug.Log(e.ToString());
		}
	}
	
	/// <summary>
		/// Emit the pack or message to server.
		/// </summary>
		/// <param name="callbackID">Callback ID.</param>
		/// <param name="_pack">message</param>
		public void EmitBytesToServer(string callbackID, byte[] _pack)
		{

			try{

				if(udpClientState == UDPClientState.CONNECTED)
				{
					lock ( udpClientLock) {
						
					 if(udpClient == null)
			          {
					   
					      udpClient = new UdpClient ();
				  
                          udpClient.Client.SetSocketOption(
					      SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

					      udpClient.ExclusiveAddressUse = false;
                      
						try
                        {
               	           udpClient.Client.DontFragment = false;
                        }
                        catch { }

                        try
                       {
                         const int SIO_UDP_CONNRESET = -1744830452;
                         udpClient.Client.IOControl(SIO_UDP_CONNRESET, new byte[1], null);
                       }
                        catch { } // Only necessary on Windows
					   
					IPEndPoint localEp = new IPEndPoint(IPAddress.Any, 0);
				    udpClient.Client.Bind(localEp);

				
			           }

						udpClientState = UDPClientState.SENDING_MESSAGE;
						
						byte[] byteID = Encoding.UTF8.GetBytes ("BYTE"); //convert to bytes
					
						byte[] callbackID_bytes = Encoding.UTF8.GetBytes (callbackID); //convert to bytes
					
						 // Add the files to a list.
                         IList<byte[]>  input_pack = new List<byte[]>();
						 input_pack.Add(byteID);
                         input_pack.Add(callbackID_bytes);
						 input_pack.Add(_pack);
					
                         // Serialize.
                         byte[] bytes;
						 
						  using (MemoryStream ms = new MemoryStream())
                          {
    
                            BinaryFormatter formatter = new BinaryFormatter();
                            formatter.Serialize(ms, input_pack);
                            bytes = ms.ToArray();
						  }
                       
						byte[] data = bytes;
					
						var endPoint = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);
                       
						udpClient.Send(data, data.Length,endPoint );
		
			            udpClientState = UDPClientState.CONNECTED;
						

						udpClientState = UDPClientState.CONNECTED;
					}
				}
			}
			catch(Exception e) {
				Debug.Log(e.ToString());
			}
		}





	private void OnDestroy() {
	
	 switch (peerType) {

			case PeerType.Client:
			
			break;

			case PeerType.Server:
				
			break;

		} //END_SWITCH
		lock (udpClientLock) {
			if (udpClient != null) {
				udpClient.Close ();
			}
		}

		if (clientListenner!=null) {
				
				clientListenner.Abort ();
			}
	}

	public void Update()
	{   
		try{
		lock(eventQueueLock){ 
		    while(eventQueue.Count > 0)
			{
			  InvokEvent(eventQueue.Dequeue());
			}
        }
		}
		catch(Exception e)
		{
			 Debug.LogError(e.ToString());
		}
		
	}
	
	/************************************ SERVER CODE ***************************************************************/
	public string GetServerStatus()
		{
			switch (udpServerState)
			{
			    case  UDPServerState.DISCONNECTED:
				 return "DISCONNECTED";
				break;

			    case  UDPServerState.CONNECTED:
				 return "CONNECTED";
				break;

			    case  UDPServerState.SENDING_MESSAGE:
				 return "SENDING_MESSAGE";
				break;

			    case  UDPServerState.ERROR:
				 return "ERROR";
				break;
			}

			return string.Empty;
		}

		//get local server ip address
		public string GetServerIP() {

			string serverIP = string.Empty;

			string address = string.Empty;

			string subAddress = string.Empty;

			IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

			//search WiFI Local Network
			foreach (IPAddress ip in host.AddressList) {
				
				if (ip.AddressFamily == AddressFamily.InterNetwork) {

					if (!ip.ToString ().Contains ("127.0.0.1")) {
						address = ip.ToString ();
					}
						
				}
			}
				
			if (address == string.Empty)
			{
				
				return string.Empty;
			}
			else
			{
				

				return address;
			}
			return string.Empty;


		}

		
		/// <summary>
		/// Creates a UDP Server in in the associated client
		/// called method when the button "start" on HUDCanvas is pressed
		/// </summary>
		public void CreateServer()
		{
			
					if (!serverRunning) {
						
						StartServer (serverPort);

						serverRunning = true;

						Debug.Log ("UDP Server listening on IP: "+GetServerIP()+" and port " + serverPort);

						Debug.Log ("------- server is running -------");

					}
							
		}



		/// <summary>
		/// Starts the server.
		/// </summary>
		/// <param name="_serverPort">Server port.</param>
		public void StartServer( int _serverPort) {


			if ( serverListenner != null && serverListenner.IsAlive) {
				
				CloseServer();

				while (serverListenner != null && serverListenner.IsAlive) {}
			}
				
			// set server port
			this.serverPort = _serverPort;

			// start  listener thread
			serverListenner = new Thread(
				new ThreadStart(OnListeningClients));
			
			serverListenner.IsBackground = true;

			serverListenner.Start();

		}


		/// <summary>
		/// Raises the listening clients event.
		/// </summary>
		public void  OnListeningClients()
		{
            try{


			udpServer = new UdpClient (serverPort);
              
			

			//udpServer.Client.ReceiveTimeout = 30000; // msec

		
			while (stopServer == false) {
				
				udpServerState = UDPServerState.CONNECTED;

				IPEndPoint anyIP = null;

			
				anyIP = new IPEndPoint(IPAddress.Any, 0);
			

				byte[] data = udpServer.Receive(ref anyIP);

				
				if(IsByteStream(data))
				{
				   // Deserialize.
                      using (MemoryStream ms = new MemoryStream(data))
                      {
                        BinaryFormatter formattter = new BinaryFormatter();
						
						IList<byte[]>  output_pack = new List<byte[]>();
					
                         output_pack =
                             (List<byte[]>)formattter.Deserialize(ms);
							
                        string callbackID = Encoding.UTF8.GetString (output_pack[1]);
							
					    //enqueue
				         eventQueue.Enqueue(new UDPEvent(callbackID,output_pack[2],anyIP));
					     
                        }
				}
				else
				{
				  	string text = Encoding.ASCII.GetString(data);

				    receivedMsg  = text;

					
				    lock (eventQueueLock) {
				
					
					//separates the items contained in the package using the two points ":" as sifter
					//and it puts them separately in the vector package []
					/*
		            * package[0]= callback_name: e.g.: "PONG"
		            * package[1]= message: e.g.: "pong!!!"
		            * package[2]=  other message for example!
			        */
					pack = receivedMsg.Split (Delimiter);
					
		            //enqueue
				    eventQueue.Enqueue(new UDPEvent(pack [0], receivedMsg,anyIP));
					receivedMsg = string.Empty;	
				   }

			
				}
				
				
			}//END_WHILE
			}
			catch(Exception e)
			{
				Debug.Log(e.ToString());
			}
		}
		
		public void EmitToClient(byte[] msg, IPEndPoint remoteEP)
		{
		   udpServer.Send (msg, msg.Length, remoteEP); // echo
		}
		
		public void SendBytesToClient(string callbackID, byte [] _bytes, IPEndPoint remoteEP)
		{
            try{
		    byte[] byteID = Encoding.UTF8.GetBytes ("BYTE"); //convert to bytes
					
			byte[] callback_name = Encoding.UTF8.GetBytes (callbackID); //convert to bytes
		
			// Add the files to a list.
            IList<byte[]>  input_pack = new List<byte[]>();
			input_pack.Add(byteID);
            input_pack.Add(callback_name);
            input_pack.Add(_bytes);
			
    

            // Serialize.
            byte[] bytes = null;
						 
            using (MemoryStream ms = new MemoryStream())
            {
    
              BinaryFormatter formatter = new BinaryFormatter();
              formatter.Serialize(ms, input_pack);
              bytes = ms.ToArray();
		    }
                       
			byte[] msg = bytes;
			
			udpServer.Send (msg, msg.Length, remoteEP);	
			}
			catch(Exception e)
			{
			 Debug.LogError(e.ToString());
			}

			
		}

		bool IsByteStream(byte[] data)
		{
		 	
			string byteID  = string.Empty;
			IList<byte[]>  output_pack = new List<byte[]>();
					
				
					
			if(data!=null)
			{

				try{
				// Deserialize.
                using (MemoryStream ms = new MemoryStream(data))
                {
                    BinaryFormatter formattter = new BinaryFormatter();
					output_pack =(List<byte[]>)formattter.Deserialize(ms);
							 
					  byteID = Encoding.UTF8.GetString (output_pack[0]);
					

                }
				}catch
				{
				 return false;
				}
			}
			if(byteID.Equals("BYTE"))
			{
			 return true;
			}
			
			return false;
					
		}
	
		
		/**
     *  DISCONNECTS SERVER
     */
		public void CloseServer() {

			udpServerState = UDPServerState.DISCONNECTED;	

			stopServer = true;
            print("--- close server ----");


			if (udpServer != null) 
			{
				udpServer.Close ();
				udpServer = null;
			}

			if (serverListenner!=null) {
				
				serverListenner.Abort ();
			}

		}
	/****************************************************************************************************************/

	void OnApplicationQuit() {

	     switch (peerType) {

			case PeerType.Client:
			 lock (udpClientLock) {
			   if (udpClient != null) {
				//udpClient.Close ();
			   }
		     }
			break;

			case PeerType.Server:
				
			break;

		} //END_SWITCH
		
	}

	/**
     * Close connection
     */
	public void disconnect() {
		

		switch (peerType) {

			case PeerType.Client:
			lock (udpClientLock) {
			if (udpClient != null) {
			//	udpClient.Close();
			}
		    }
			break;

			case PeerType.Server:
				
			break;

		} //END_SWITCH
		
	}
}
}
