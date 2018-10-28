# ConEmu / Cmder Tasks Editor

Simple GUI application that helps you edit tasks in [ConEmu][1] or in [Cmder][2] that uses ConEmu implementation.

## Motivation

I find myself pretty often creating tasks for certain development environment and I didn't really like the GUI interaction in ConEmu which made me kind of copy paste my task into a text editor and then pasting the command inside the existing textbox field.<br>
So in a situation where the command is really long, I though it would be comfortable making a GUI application to automate the process of connecting the tasks together and format the command string properly so you get a better overview in the GUI.<br>

## Notes

Since I wrote it really quick to get what I wanted accomplished, there are certain things that are not implemented yet like moving positions of the tasks, cloning, create new etc...<br>
Atm it can just edit existing tasks.<br>
If I'll see the need of making fully independed editor from ConEmu tasks editor, than I might implement it later in my free time.

## Pictures

![alt text][img_main]

![alt text][img_edit]


[1]: https://github.com/Maximus5/ConEmu
[2]: https://github.com/cmderdev/cmder
[img_main]: pictures/mainwindow.png
[img_edit]: pictures/editwindow.png