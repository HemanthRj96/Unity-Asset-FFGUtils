
# FFG Utility Assets

FFG Utility Assets is an asset bundle used for making your game developement lot faster. It contains useful assets which can replace commonly used systems in almost all kind of projects.

Details about all the assets are given below :

- [Controllers](#controllers)
    - [Animation Controller](#animation-controller)
    - [State Controller](#state-controller)
- [Managers](#managers)
    - Game Manager
    - Level Manager
    - Prefab Manager
    - Sound Manager
- [Systems](#systems)
    - Action System
    - Chrono System
    - Grid System
    - Pooling System
    - Reference System
    - Texture Data Extraction System
    - Transform Recording System
    - UI System

Modules aren't the only feature this asset provides but it also incorporates utility methods, extension methods, useful context menu options. They are shown below :
- [Other Utilities](#other-utilities)
- [Context Menu Options](#context-menu-options)


# Controllers
Controllers are used to change the local behaviour or an entities state. There two types of controllers used in this package they are listed below.

### Animation Controller
Animation controller is a component inside **FFG.Controllers** namespace. 
This is an editor inclined component which helps you to load animation clips and setup **AnimatorController**. 
It has methods to play animations easily and if a **StateController** component is attached to the same GameObject, then animation state changes can be done automatically 
by getting data from **StateController** component.
Hence this is a quick and easy workaround instead of setting up AnimatorController codes and calling them based on certain logic.

### State Controller
State controller is a component inside **FFG.Controllers** namespace. 
This component is a state machine system which automatically changes states from one to another, based on simple user provided logic.
You only have to provide certain constraints and conditions and the rest of switching logic is taken care by the controller. 
The 4 main parts of this controller are **StateController**, **StateSharedData**, **StateSyncInput** and finally **State**. 

**StateController** is the component itself that should be attached to the target gameObject. It has a custom inspector which is fairly easy to setup. This component is sealed therefore it cannot be inherited.

**StateSharedData** is a scriptable object which as the name suggests acts like a data bank which is accessible to all the **State**, therefore all shared variables or data can be put inside this 
object that is common. For example :- _playerSpeed or _jumpingForce

**StateSyncInput** is a scriptable object which is where all user input based conditions or general logical conditions should be put inside.

**State** is a scriptable object which holds all the update logic. **State** can be created by selecting from [Context Menu](#context-menu-options). Once a **State** is made from a file template then the setup includes setting up the update mode (FixedUpdate/Update)

# Managers

# Systems

# Other Utilities

# Context Menu Options
