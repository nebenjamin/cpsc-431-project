Instruction to get SVN Source Control


# Links: #

Link to Project Source
http://code.google.com/p/cpsc-431-project/source

Url of Repository
https://cpsc-431-project.googlecode.com/svn/trunk/

Link to add new members to source control
http://code.google.com/p/cpsc-431-project/adminMembers

Link to TortoiseSVN
http://tortoisesvn.net/downloads
- Download the version for you. This is windows only, you linux people will
have to find a different solution :)

SVN Merge Tool guide
http://tortoisesvn.tigris.org/TortoiseMerge.html

# Guide: #

This doc will give a very short overview of svn and our projects use of it.
First, go to the above svn link and download and install tortise svn. Next,
if you do not have a google account, you must make one to log into our source
control. I added everyone to our project who gave a gmail account already.
Once you make the account, have someone go to the new member link above and add
you, or email me at emorycook@gmail.com and I will take care of it.

Once you have a gmail account, access to the google source control, and have
installed tortoise, we are ready to download code. Make a new folder to store
your code. Right click in the folder and you should see SVN menu options.
Select "SVN Checkout". Enter the repository url from above and hit ok. Then go to
the project source link and click the "google.com password" link to get your random
password. The username will be the username from your google account. If any code is present,
it should download now.

Right click in the folder and select TortoiseSVN -> Settings. In the general menu
you will see a filters box. This gives ignore patterns so lame files are not uploaded.
I generally use **.swp Thumbs.db because gvim creates .swp files which i dont want to accidently add,
and everyone knows the pain of windows and its thumbs.db. If you think of any more, add them.**

When you create a file, right click the file and select tortioseSvn -> add. This will mark the file
for addition to the source control. Next time you commit your changes, the new file will be added.
To remove a file from source control, you must use tortiose svn -> Delete. If you want to rename a file,
you must do it from the svn file. If you move a file, delete it, rename it or anything using
standard windows controls, svn will just assume you need to have the file added to your directory again
next time you update. If you are editing a file and you mess up, select tortoise svn -> revert.
This grabs the latest copy and overwrites your changes.

# SVN Best Practices: #

Always update before you commit! This way, you can make sure that someone did not make a bug fix or
change which breaks yourcode!

Always compile and test before you commit! You do not want to be the guy who breaks the build and have
people call you at midnight to fix your bugs!

Commit small complete chunks when possible. The longer you go between commits, the further your code
grows from the project. With 16 people working on this, there is going to be a lot of overlap.

When there is a conflict, check the diff and know that you are not always correct! Check out the guide
to the svn diff tool and ask if you have any questions. If you are not sure about changes, ask
the guy who made the change.

Always leave a GOOD commit note! These allow us to know what we are changing and trace back who did
what when we have questions.