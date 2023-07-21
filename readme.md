# Desktop Pet for Personal Code Management
## Introduction
This is a customizable desktop pet for code management. You can save, update, delete or check your codes, paths, tricks or whatever you may frequently access with this tool. You are also encouraged to create and share new features based on this project.

## Method
The project is based on sqlite. The user can save any string into the database, and some simple commands make accessing these strings easier.

## Commands
Users can access the various functions with the command line, which can be accessed by double-clicking the pet(or by activating the pet window and pressing ENTER).

Listed below are some of the common commands supported by the current version.
### Command Handler
#### Help
```
help [cmd]
```
##### Print help strings of a specific command or of all commands.
##### Input:

* cmd: Command to query.
#### History
```
history [str]
```
##### Show histories containing a certain string or show all histories.
##### Input:

* str: String to look for.

### Commone Database Commands
#### Save
```
save/remember/memorize table_name [name] [contents]
```
##### Saves something to the database.
##### Input:
* table_name: Name of the table. It is recommended that you put strings for different usages into different tables. By default, tables include "code", "path", "exe", "tables" and "project". You can also define your tables with the "create_table" command.
* name: Name of the entry.
* contents: The string you want to save. If table_name is "code", contents should be a file name to be saved. If table_name is "path", contents should be the relative path to the current working path.
#### Usage
A confirmation dialog will start and you can define the title, description and contents of the entry. Pressing ENTER in the dialog(note that neither the description box nor the contents box can be selected) represents confirmation, while pressing ESC means cancelling the action. The very controls are applied to other actions like update, copy, delete etc.

#### Filter
```
find/recall/filter/get/li table_name [filter_string]
```
##### Look for something in the database.
##### Input:
* table_name: Name of the table.
* filter_string: The filter string. Examples only: "nn" for listing all entries with "nn" in the name; "nn;desc:python" for listing all entries with "nn" in the name and "python" in the description.
#### Usage
This command fetches and caches a list of results from the table. The results are shown by lines each containing an index and the summary of the entry.
Some other actions like update, copy, delete etc. rely on the result of this command.


#### Update
```
update/fix/change/detail/+ [idx]
```
##### Changes something in the database.
##### Input:
* idx: Default 0. Index of the entry in the list to be updated.
#### Usage
A filter action MUST be done in advance.
A confirmation dialog will start and you can change the title, description and contents of the entry.

#### Remove
```
del/delete/remove/forget/- [idx]
```
##### Removes something from the database.
##### Input:
* idx: Default 0. Index of the entry in the list to be deleted.
#### Usage
A filter action MUST be done in advance.
A confirmation dialog will start and you can confirm or cancel the action.

#### Copy
```
copy/fetch [idx] [file_name]
```
##### Changes something in the database.
##### Input:
* idx: Default 0. Index of the entry in the list to be copied.
* file_name: File to write the string. If the file name is not specified, the string is copied to your clipboard.
#### Usage
A filter action MUST be done in advance.
A confirmation dialog will start and you can confirm or cancel the action.

### Tools
#### Calculator
```
calc string
```
##### Calculation.
##### Input:
* string: Mathematic expressions like "1+2", "(3+4)*5".
#### StartUp
```
startup
```
#### Changes startup commands.
#### Usage
Edits a "startup.ini". All lines are executed as if the user inputs them as normal command lines.
### Special Database Commands

* Please refer to the code. README not finished.