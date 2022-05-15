# NVROrganizer

Version 2.0 attributes:

This App is a Vampped up version of the NVR Organizer,





Version 1.0 attributes:

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

Special Thanks to Thomas Claudius Huber and Ernesto Ramos for their Contributions.
