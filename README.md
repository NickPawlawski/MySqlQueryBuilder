# MySqlQueryBuilder

This is a c# library built to allow developers to quickly build queries and execute them with transactional support and feature rich reporting.

This library was inspired by Laravel's Eloquent ORM and tried to share some of its ideas for the building blocks of the code. 

# Using the library

Each of the four queries , insert, update, delete, and select, has its own syntax. Each will be displayed below. 

### Initializing the library

To initialize the library, call the function SetServerString within the DatabaseConnection class and pass it the parameters

> server      
> userid      
> password    
> database    

Once this method is called it does not need to be called again. You can now use the rest of the software. 

#### The Parent

The parent builder is the class responsible for managing a single transaction. Create a parent using the parentbuilder constuctor. 

`
ParentBuilder _pb = new ParentBuilder();
`

#### The Child

The child builder class is responsible for a single query. Each child gets loaded into the parentbuilder class. When the parent runs 
each of the childs queries are ran in the sequence they were loaded into the parent. Here is an example of creating a new child. 
The parameters of creating a child are the table name, the dictionary containing the details of the query then what query we want to run.

`
new ChildBuilder("categories", dict, ParentBuilder.Statements.Select);
`

#### Select
Creating a select statement has never been easier. First start by using the dictionary builder class to build a dictionary using the 
column names. In this example we want to look at the colums CategoryID, CategoryID, and Picture.

`
Dictionary<string, List<string>> dict = DictionaryBuilder.BuildDictionary(new List<string>() { "CategoryID", "CategoryName", "Picture" });
`

Once we have built the dictionary we create a child with that dictionary. 

`
new ChildBuilder("categories", dict, ParentBuilder.Statements.Select);
`

Once the child is created we can add it to the parent. I usually choose to do this in one step that looks like this.

`
_pb.AddChild(new ChildBuilder("categories", dict, ParentBuilder.Statements.Select));
`

Repeat this step or add any of the other queries needed to the parent, then use the run method to run the queries.

`
_pb.Run();
`

All together one select query can look like this,

`

QueryBuilder.DatabaseConnection.SetServerString("localhost","root","root","northwind");

ParentBuilder _pb = new ParentBuilder();

Dictionary<string, List<string>> dict = DictionaryBuilder.BuildDictionary(new List<string>() { "CategoryID", "CategoryName", "Picture" });
_pb.AddChild(new ChildBuilder("categories", dict, ParentBuilder.Statements.Select));

_pb.Run();

`
