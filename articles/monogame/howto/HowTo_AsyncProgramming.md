---
title: How to work with Asynchronous Methods in MonoGame
description: This topic describes how you can work with asynchronous methods in MonoGame.
---

# Working with Asynchronous Methods in MonoGame

This topic describes how you can work with asynchronous methods in MonoGame.

MonoGame provides many methods that operate _asynchronously_ for operations that may take longer than the desired render-cycle length.

Asynchronous methods consist of four elements:

* A **Begin** call that begins the asynchronous process. **Begin** methods return an [IASyncResult](http://msdn.microsoft.com/en-us/library/system.iasyncresult.aspx) object that can be used to poll for completion if a callback function is not used to detect the completion of the operation.
* An **End** call that ends the asynchronous process and returns objects or data requested by the **Begin** call. Calling the corresponding **End** method for each **Begin** method is important to prevent deadlocks and other undesirable behavior.
* An optional _callback_ method that is called by the system when the asynchronous operation completes. This is passed to the **Begin** call.
* An optional, arbitrary _tracking object_ that can be supplied to **Begin** to uniquely identify a particular asynchronous request. This object is part of the _IASyncResult_ returned by **Begin**, and is also present in the callback method's _**IASyncResult**_ parameter. Because of this, it also can be used to pass arbitrary data to the callback method when the asynchronous process completes.

The two most common methods of working with asynchronous methods are to check for completion by polling or by callback. This topic describes both methods.

For exhaustive information about asynchronous methods, see [Asynchronous Programming Design Patterns](http://msdn.microsoft.com/library/ms228969.aspx) on MSDN.

## To poll for asynchronous method completion

1. Call the asynchronous **Begin** method, and save the returned _**IASyncResult**_ object to a variable that will be checked for completion.

2. In your update code, check [IsCompleted](http://msdn.microsoft.com/en-us/library/system.iasyncresult.iscompleted.aspx).

3. When [IsCompleted](http://msdn.microsoft.com/en-us/library/system.iasyncresult.iscompleted.aspx) is **true**, call the **End** method that corresponds to the **Begin** method called in step 1.

### To use a callback to check for asynchronous method completion

1. Call the asynchronous **Begin** method, passing it an [AsyncCallback](http://msdn.microsoft.com/en-us/library/system.asynccallback.aspx) method that will be called when the asynchronous process is completed.

    > [AsyncCallback](http://msdn.microsoft.com/en-us/library/system.asynccallback.aspx) methods must return void, and take a single parameter: [IASyncResult](http://msdn.microsoft.com/en-us/library/system.iasyncresult.aspx).

2. In the callback, call the **End** method that corresponds to the **Begin** method called in step 1.

    > The **End** method typically returns any data or objects requested by the **Begin** call.

## See Also

[Asynchronous Programming Design Patterns](http://msdn.microsoft.com/library/ms228969.aspx)  

---

© 2012 Microsoft Corporation. All rights reserved.  

© 2023 The MonoGame Foundation.
