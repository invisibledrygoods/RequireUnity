RequireUnity
============

Modular design tools for unity.

Example
-------

#### Declare resources

    Transform module;
    UsesSelect selector;
    HasPoints hasPoints;

#### Make sure we have access to all the resources we need

    void Awake()
    {
        module = transform.GetModuleRoot();
        hasPoints = module.Require<HasPoints>();
        selector = transform.Require<UsesSelect>();
    }
    
How It Works
------------

#### transform.Require&lt;T : Component&gt;

Require works similarly to ruby's require, it checks to see if the transform already has a component of the given class, and uses AddComponent to create one if it doesn't.

#### transform.GetModuleRoot

GetModuleRoot searches up the scene heirarchy looking for a GameObject with a `ModuleRoot` component, returning a reference to its transform if found, and returning the calling transform if not found.

#### transform.GetModuleRoot().Require&lt;T : Component&gt;

By requiring on the module root instead of your own transform multiple child GameObjects can share their parents resource. This lets you centralize the data of a game entity but organize its control into child GameObjects.

Use In Testing
--------------

Between instantiating a game object and requiring the component under test, there is a window of opportunity to inject mock components that extend the component under test's required resources. When require goes to look for the resources it will find the injected mock and never load the original resource.

This is useful for cases where the original resource would modify global state and cause interference between tests, or where the original resource would access foreign data and potentially corrupt save files or database entries. By injecting a stateless mock you can isolate your tests from each other and the outside world.

#### It looks kind of like this

    class Database {
      public void Write(string entry) {
        // write directly into the new york stock exchange's list of fortune 500 CEOs
      }
    }

    class MockDatabase : Database {
      public string lastEntry;
      
      public void Write(string entry) {
        // just remember the last entry quietly without crashing the stock market
        lastEntry = entry;
      }
    }
    
    class NameWriter {
      Database db;
      
      void Awake() {
        db = transform.Require<Database>();
      }
      
      public void WriteName() {
        db.Write("Boogersnot Jones")
      }
    }

    class NameWriterTest {
      void TestNameWriter()
      {
        // make sure the fake database is already in place when NameWriter goes to look for it
        MockDatabase mock = transform.Require<MockDatabase>();
        
        // require NameWriter after all its mock dependencies are in place
        NameWriter writer = transform.Require<NameWriter>();
        
        // this should now write safely to our mock database instead of the real one
        writer.WriteName();
        
        // and we can even stick spy code into the mock to see how our NameWriter did at its task
        mock.lastEntry.ShouldBe("Boogersnot Jones");
      }
    }

More Reading
------------

The motivation behind Google Guice is the same as the motivation behind Require. There's also just a lot of good stuff in here: https://code.google.com/p/google-guice/wiki/Motivation
