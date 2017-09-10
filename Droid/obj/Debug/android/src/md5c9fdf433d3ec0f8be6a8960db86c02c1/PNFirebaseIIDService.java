package md5c9fdf433d3ec0f8be6a8960db86c02c1;


public class PNFirebaseIIDService
	extends com.google.firebase.iid.FirebaseInstanceIdService
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onTokenRefresh:()V:GetOnTokenRefreshHandler\n" +
			"";
		mono.android.Runtime.register ("Plugin.FirebasePushNotification.PNFirebaseIIDService, Plugin.FirebasePushNotification, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", PNFirebaseIIDService.class, __md_methods);
	}


	public PNFirebaseIIDService () throws java.lang.Throwable
	{
		super ();
		if (getClass () == PNFirebaseIIDService.class)
			mono.android.TypeManager.Activate ("Plugin.FirebasePushNotification.PNFirebaseIIDService, Plugin.FirebasePushNotification, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onTokenRefresh ()
	{
		n_onTokenRefresh ();
	}

	private native void n_onTokenRefresh ();

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
