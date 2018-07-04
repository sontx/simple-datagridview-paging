**simple-datagridview-paging** is a simple UserControl that shows the data-table and paging automatically.
With this library, you can also edit the data and it will be saved to the database automatically.
![](https://3.bp.blogspot.com/-76IqcXrupWc/Wzzowu0OxNI/AAAAAAAAVRs/J2Xwqp4aN7ce02rEfMB47HmlGxQpwoYOgCLcBGAs/Capture.PNG)

Only ONE line:
``` cs
dataGridViewPaging.DbRequestHandler = DbRequestHandlerHelper.Create(new SQLiteConnection("Data Source=chinook.db"), "tracks");
```

## Installation
Nuget:

> Install-Package SimpleDataGridViewPaging

Or download [binary file](https://github.com/sontx/simple-datagridview-paging/releases).

## Usage

1. Add the libary to the ToolBox: Right click to **ToolBox** -> **Choose Item...** -> In **.Net Framework Components**, click on **Browse...** and select the **SimpleDataGridViewPaging.dll** file then click **OK**.
2. The **DataGridViewPaging** will be shown in the **ToolBox** like this: ![](https://camo.githubusercontent.com/a9e6ae4695fdac87b32f247b001bbd00622cd1bb/68747470733a2f2f6c68332e676f6f676c6575736572636f6e74656e742e636f6d2f2d7448727779656c73724a672f5636625778556b79484c492f414141414141414150646f2f713473314163794c636b59696e61527054545a4235316f70697941704252345077434b67422f73302f436170747572652e504e47)
3. Drop&drag this control into your form.
4. Code:
``` cs
dataGridViewPaging.DbRequestHandler = DbRequestHandlerHelper.Create(new SQLiteConnection("Data Source=chinook.db"), "tracks");
```

> There are two parameters: the connection and the table name that will be shown in the control.

5. Enjoy ;)

### More Options
There are several custom levels:
1. Read-only mode:

``` cs
// The first parameter is the database connection,
// the second one is the table name the will be queried and shown in the control.
dataGridViewPaging.DbRequestHandler = DbRequestHandlerHelper.Create(
    new SQLiteConnection("Data Source=chinook.db"),
    "tracks");
```

2. Editable mode:
``` cs
// Like the readonly mode but the third parameter is the CommandBuilder object. 
dataGridViewPaging.DbRequestHandler = DbRequestHandlerHelper.Create(
    new SQLiteConnection("Data Source=chinook.db"),
    "tracks",
    new SQLiteCommandBuilder(new SQLiteDataAdapter()));
```

3. Custom query text:
``` cs
// The libary will use this statement to query the number of records
// that need to caculate the pagination info.
var countStatementBuilder = new CountStatementBuilder();
countStatementBuilder.CommandText("SELECT COUNT(*) FROM tracks");

// This statement will be used to query the actually data of the table.
// There are 3 placeholders: {0} - table name, {1} - max records, {2} - page offset.
// The libary will pass corresponding data to these placeholders on demand.
var rowsStatementBuilder = new RowsStatementBuilder();
rowsStatementBuilder.CommandText("SELECT * FROM tracks LIMIT {1} OFFSET {2}");

dataGridViewPaging1.DbRequestHandler = new DbRequestHandler
{
    Connection = new SQLiteConnection("Data Source=chinook.db"),
    CountStatementBuilder = countStatementBuilder,
    RowsStatementBuilder = rowsStatementBuilder
};
```

4. Manual querying. [Take a look](https://github.com/sontx/simple-datagridview-paging/blob/master/Examples/ManualQueryWithReadOnlyForm.cs)

## Contributing
1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request :D

## Dependencies

 - .Net Framework 4.5 or later.

## Author
Developed by sontx/noem, some useful information:

 - Home: [www.code4bugs.com](https://code4bugs.com)
 - Blog: [https://sontx.blogspot.com](https://sontx.blogspot.com)
 - Email: <a href="mailto:xuanson33bk@gmail.com">xuanson33bk@gmail.com</a>
 - Twitter: [@sontx0](https://twitter.com/sontx0)

## License
[MIT](https://github.com/sontx/simple-datagridview-paging/blob/master/LICENSE)