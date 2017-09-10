package md5c9fdf433d3ec0f8be6a8960db86c02c1;


public class PNFirebaseMessagingService
	extends com.google.firebase.messaging.FirebaseMessagingService
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onMessageReceived:(Lcom/google/firebase/messaging/RemoteMessage;)V:GetOnMessageReceived_Lcom_google_firebase_messaging_RemoteMessage_Handler\n" +
			"";
		mono.android.Runtime.register ("Plugin.FirebasePushNotification.PNFirebaseMessagingService, Plugin.FirebasePushNotification, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", PNFirebaseMessagingService.class, __md_methods);
	}


	public PNFirebaseMessagingService () throws java.lang.Throwable
	{
		super ();
		if (getClass () == PNFirebaseMessagingService.class)
			mono.android.TypeManager.Activate ("Plugin.FirebasePushNotification.PNFirebaseMessagingService, Plugin.FirebasePushNotification, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onMessageReceived (com.google.firebase.messaging.RemoteMessage p0)
	{
		n_onMessageReceived (p0);
	}

	private native void n_onMessageReceived (com.google.firebase.messaging.RemoteMessage p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
