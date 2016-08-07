What is this?
=======

This is a library provides a simple UserControl contains a DataGridView and a BindingNavigator. So, it can automatically paging in a simplest way.

![](https://lh3.googleusercontent.com/-40KA0138bXw/V6bUTgNBuWI/AAAAAAAAPc8/Cm8sqCftMKgMYySzMyw4imfRtMSL4IiEgCKgB/s0/Capture.PNG)

Cool! How to use it?
=======

Fist step, add library
---------
Add this libary to your project. This is easy step, you just do something like below photo or use nuget.

![](https://lh3.googleusercontent.com/-xKPzYiCE05A/V6bVvBL_hHI/AAAAAAAAPdU/9hCPUXymrtIQ5Vx99yWiNP66q7XkDpCfwCKgB/s0/Capture.PNG)

Second step, drag-drop to form
-----------
Open your form in design mode(where you can drag-drop controls to design UI). See Toolbox, *DataGridViewPaging* control has been placed here. Just drag this control and then drop in your form where you want. It's similar other controls such as TextBox, Button, CheckBox....

![](https://lh3.googleusercontent.com/-tHrwyelsrJg/V6bWxUkyHLI/AAAAAAAAPdo/q4s1AcyLckYinaRpTTZB51opiyApBR4PwCKgB/s0/Capture.PNG)

Third step, coding
--------
This step depends on your choices. This UserControl provides two ways to process request query are auto and manual.

**Easiest way**, let *DataGridViewPaging* do everything for you.
``` cs
var connection = new SQLiteConnection("Data Source=chinook.db");
connection.Open();
dataGridViewPaging.UserHardMode(connection, "tracks");
```
Result of this way.
![## Heading ##](https://lh3.googleusercontent.com/-qvi740z9yzQ/V6beNdv46AI/AAAAAAAAPeg/LFWsZVJP5GUPBhrCIk_ozQOy6kVqbZbfQCKgB/s0/Capture.PNG)

**A little more option**, you can define the query string for *DataGridViewPaging* instead of let it auto define.
``` cs
var connection = new SQLiteConnection("Data Source=chinook.db");
connection.Open();
dataGridViewPaging.UserSoftMode(connection, "SELECT Name, Composer, Bytes FROM tracks WHERE TrackId < 500");
```
Result of this way.
![](https://lh3.googleusercontent.com/-Fb5ukCRhGpc/V6bd8Gz_IiI/AAAAAAAAPeQ/XdBIKSH2kt4SkNtsAHxkZ8fAxW5pRrSQQCKgB/s0/Capture.PNG)

**Let you do everything**. You have to query database when *DataGridViewPaging* on demand.
``` cs
var connection = new SQLiteConnection("Data Source=chinook.db");
connection.Open();
// register request query event
dataGridViewPaging.RequestQueryData += DataGridViewPaging_RequestQueryData;
// set number of rows(records) and start query data
using (var command = connection.CreateCommand())
{
    command.CommandText = "SELECT COUNT(*) FROM tracks";
    var reader = command.ExecuteScalar();
    dataGridViewPaging.Initialize(Convert.ToInt32(reader));
}

private void DataGridViewPaging_RequestQueryData(object sender, RequestQueryDataEventArgs e)
{
    // query data and then set result to display
    using (var command = connection.CreateCommand())
	{
        command.CommandText = "SELECT * FROM tracks LIMIT " + e.MaxRecords + " OFFSET " + e.PageOffset;
        dataGridViewPaging.DataSource = command.ExecuteReader();
    }
}
```
 Result of this way.
![](https://lh3.googleusercontent.com/-bdascVImdBY/V6bdluVT1lI/AAAAAAAAPeA/fOmi1v8RYa8X4KJqSg50C9oOmySqXRsuwCKgB/s0/Capture.PNG)

Wait! How can I get this libary?
=========

Two ways.

Nuget.org
---------
This libary is already published to this location [SimpleDataGridViewPaging](https://www.nuget.org/packages/SimpleDataGridViewPaging/). 
You can install by using this command:

    Install-Package SimpleDataGridViewPaging
Also, a easier way is using *NuGet Packages* GUI in Visual Studio like this photo.
![](https://lh3.googleusercontent.com/-VxWDksS-s4w/V6bpg8roIQI/AAAAAAAAPfE/xiIyCaAZTMUj3VJanmnU1W-__6jsyTfKACKgB/s0/Capture.PNG)

Github.com
----------
Of course, you can download standalone version here: [https://github.com/sontx/simple-datagridview-paging/releases](https://github.com/sontx/simple-datagridview-paging/releases)
It contains source code and a binary file for library(compiled in net45).
