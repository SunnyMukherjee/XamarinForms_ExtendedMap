# XamarinForms_ExtendedMap
Project shows additional, useful features on top of the default Xamarin Map control.

The Xamarin Map control is a great way to make your app even more attractive to your users if you are showing location data.  But the default map control comes with its limitations.  It does not have a default way of centering the map on the user's current location and configuring the distance radius from the center position.  This sample project shows how to extend and make the map control even more useful.  It centers and configures the zoom distance but does not add any test pins onto the map itself.  You can use the ExtendedMap control in your projects to add pins dynamically because it is tested and working.

This sample project assumes that you know how to set up and configure the Xamarin Maps control in your projects.  If you have not done it yet, follow the steps below.
https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/map/setup

Please note you have to follow a few extra steps on Android, including setting up an Api Key.  Follow the steps below.
https://docs.microsoft.com/en-us/xamarin/android/platform/maps-and-location/maps/obtaining-a-google-maps-api-key

CAVEATS

- If you test in a simulator, remember to enable the location in the simulator settings to get a sample location.  This will simulate a user's current location on a real device.
- To run it on the Android simulator, after setting up the Maps SDK and API in the Google API Console, paste the API key in the API_KEY in the AndroidManifest.xml.
