# UwpDemoApp

Dummy UWP application using LiteDB to show problem with encrypted database password change.

LiteDB password change issue reproduction scenario:

- Fill in "Enter current password" with a new password and click "Open DB". It will set the DB password on the first run.
- Fill "Enter text to write" with any text you like to push it into DB. Click "Write text".
- Now both DB data-file and log-file are created.
- You can check if the data are written successfully. Press "Read text from DB" to read all contained strings of text.
- Fill in "Enter new password" with the new DB password and click "Change DB password".
- After this, you can still successfully read and write data to DB. You can check it.
- Close application

- You can take a look at the DB log-file that remains to exist. Dispose() will be called later whenever GC decides to be necessary.

- Open-up application again
- Fill in "Enter current password" with your changed password and click "Open DB". It will open DB successfully.
- Press "Read text from DB" to read all contained strings of text successfully.
- Fill "Enter text to write" with any text you like to push it into DB. Click "Write text".
- You'll see the "Invalid password" exception.
