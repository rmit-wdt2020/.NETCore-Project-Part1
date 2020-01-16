Group 19

Caecario Andika Wardana		S3711914 
Ming Yuan Tan			S3739706




Why did you use these patterns? What purpose and advantage do they offer? Any other important points. 

Pattern #1 Factory 
The Factory pattern provides an interface for creating objects in a superclass, but allows subclasses to alter the type of objects that will be created.
We used it in for our Account class to create objects of child classes Savings and Checking Accounts. The advantage is that now you we can override the 
factory method and change the class of products created by the method if we were to expand the type of accounts we'll offer in the future e.g. Joint Accounts. 

Pattern #2 Singleton 
Singleton lets you ensure that a class only has one instance, while providing global access to that instance. We set our BankingSys class as singleton 
as it contains the List of Customer and Login objects. We want to make sure that we are calling the same instance of those objects and not a duplicate so 
that data can be manipulated accurately. 

Pattern #3 Facade 
The facade pattern provides a simplfied interface to the overall project. It allows the facade to encapsulate the functionality, and hides it from the rest
of the code. We used it to encapsulate the program class, to simplify the code, and make the program class cleaner.