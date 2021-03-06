# NVROrganizer

## Version 2.0 attributes:

### This WPF Enterprise App with MVVM is a Vampped up version of the NVR Organizer V1.0, it incudes:

- Setting up a DataBase with Entity Framework Code 1st

- Detecting Model changes with Entity Framework db. Context

- Validating User Input with Data Annotations and INotifyDataErrorInfo

- Implementing Optimistic Concurrency to handle Multi-User Conflicts (DataBase Wins Vs. User Wins)

- Implemented Communication between ViewModels with Prism's EventAggregator

- Tabbed User Interface 

- A Drop down menu to add the Program that is used to access the NVRs and a way to Edit the list 

- Added a Section to create appointments that displays a Calendar and PickList

- Modern dark design (Mahapps.Metro for Styling)

*Find File :namespace NvrOrganizer.UI.Wrapper.App.xaml in the Solution Explorer, Line 14*
```  <ResourceDictionary Source="pack://application:,,,/MahApps.Metro;component/Styles/Themes/Dark.Crimson.xaml" /> ```

*For a different Theme, Change the word Crimson to: *

``` "Red", "Green", "Blue", "Purple", "Orange", "Lime", "Emerald", "Teal", "Cyan", "Cobalt", "Indigo", "Violet", "Pink", "Magenta", "Crimson", "Amber", "Yellow", "Brown", "Olive", "Steel", "Mauve", "Taupe", "Sienna" ```

 ### Special instructions required for the reviewer:
 
*Please Build it, if successful, Select NVROrganizer.DataAccess as the Default Project than ``` update-Database ``` in the Package Manager Console*

### Ernesto's Way

- run ``` update-database -Target 0 ``` to rollback all the migrations.
- Then deleted all the migrations.
- Run ``` add-migration InitialDatabase ``` to create a new migration with all the database changes.
- Run ``` update-database ```
- Run the Application


### *Worst case Scenario*

Delete all Migrations (except Configuration.cs), located in the Migrations folder under the NVROrganizer.UI Solution from the Solution Explorer

### Go to the Package Manger Console Tab and add these Migrations:

- ``` Add-Migration InitialDatabase ```           *(Wait for it to seed)*
- ``` Add-Migration AddedNvrPhoneNumbers ```      *(Wait for it to seed)*
- ``` Add-Migration AddedMeetings ```             *(Wait for it to seed)*
- ``` Add-Migration AddedProgrammingLanguage ```  *(Wait for it to seed)*
- ``` Add-Migration AddedRowVersionToNvr ```      *(Wait for it to seed)*
- ``` Update-Database ```                         *(Run this code after each Migrations & Wait for it to seed)*
- Run the Application



## Version 1.0 attributes:

The App retrieves the clients First name, Last Name and the requried Alias from a Local DataBase, clicking on the Left Navagation will populate the data on the right side.

All the Classes inherits one or more properties from its parent, If any information is changed, the Save button will be active and the user will have the option to
up date the dataBase, save, and Store it for future use. 

The First and Last name has a limit of 50 characters, if there are more than 50 characters, the app will inform the user of the error. the First name is always required.

If the user makes a change to the clients info and doesn't save it, (by navagating to a different client on the right navagation view) a pop box will inform the user of 
the potential loss. If the info isn't Saved, there is no change in the DataBase, if there is a Save, the Navagation view on the right will update and will be saved to the
DataBase.

There is a Create button above the list of clients, this will allow the user to add more clients to the DataBase, the First name will default to a required Field, After a 
Save, the DataBase will update and the new client will be displayed on the right side Navagation view.

There is also a Delete button to clean up the DataBase when a client is no longer needed. When the Delete button is used, a Pop up box will appear and ask the user to
confirm the delete action.

As the DataBase gets bigger the code uses the LoadAsync Properties to keep the App Responsive by only loading the necessary information to help with the 
computers Memory usage.

### Special Thanks to *Thomas Claudius Huber* and *Ernesto Ramos* for their Contributions.
