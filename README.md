# Custom alias plugin for lethal company

### Summary

- [To add with LethalConfig](#to-add-with-lethalconfig)
- [To remove with LethalConfig](#to-remove-with-lethalconfig)
- [How to add/remove/edit by hands?](#how-to-addremoveedit-by-hands)

## Goal

To create a fully customisable alias system.

## How does it work?

Aliases are registered with a unique string, as shown in the default configuration below.  
```alias keyword : command``` This is the structure of an alias binding, and each alias binding is separated by a semicolon ```;```.

# To add with LethalConfig

To add your alias, you can go on the "Add" section, put your key/value, and push the button add.
![Add](https://github.com/FlaveFlav20/AliasPlugin-Lethal-Company/blob/main/gifs/add.gif?raw=true)

# To remove with LethalConfig

To remove, you can go to your alias, and click to remove.
![Remove](https://github.com/FlaveFlav20/AliasPlugin-Lethal-Company/blob/main/gifs/remove.gif?raw=true)

# To search

To add your alias, you can just put your key/value and push the button add.
![search](https://github.com/FlaveFlav20/AliasPlugin-Lethal-Company/blob/main/gifs/search.gif?raw=true)

## How to add/remove/edit by hands?

Characters ```:``` and ```;``` are prohibited in alias/command. You can edit the configuration file by following this rule.  
To add an alias, insert ```your alias: your command``` into the configuration file.  
To delete an alias, remove the line containing the target alias and its associated command.  
Don't forget to separate your alias by a semicolon ```;```.

## Default configuration (List alias)

```vm : VIEW MONITOR ; sw : SWITCH ; s : SWITCH ; p : PING ; t : TRANSMIT ; sc : SCAN ; st : STORE ; m : MOONS ; tcb : THE COMPANY BUILDING ; exp : EXPERIMENTATION ; ass : ASSURANCE ; v : VOW ; ma : MARCH ; off : OFFENSE ; ad : ADAMANCE ; re : REND ; di : DINE ; ti : TITAN```
