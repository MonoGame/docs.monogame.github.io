
# Android

![](images/game_android_target.png)

![](images/game_android_device.png)

![](images/game_android_device2.png) 

![](images/game_android_orientation.png)

# iOS Signing

XCode --> Settings

![](images/xcode_certificates.png)

![](images/xcode_signing.png)

```
<PropertyGroup Condition=" '$(Configuration)' == 'Release' ">
    <CodesignKey>iPhone Distribution</CodesignKey>
</PropertyGroup>

<PropertyGroup Condition=" '$(Configuration)' == 'Debug' ">
    <CodesignKey>iPhone Developer</CodesignKey>
</PropertyGroup>
```

# Debugging

![](images/game_ios_simulator.png)

