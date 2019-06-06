# Adding GAM to your path

This should have happened when you installed GAM, but here are some instructions to do it now.

1. Search start for "Advanced system settings" (you can also run sysdm.cpl and go to the advanced tab)
2. Click on Environment Variables
3. In the System Variables section, double click PATH.
4. Click on New...
5. Add the absolute (with C:\) path to your GAM folder. This is C:\GAM by default, but if you changed it in the GAM installer enter that path.

# Testing

To test whether GAM is in your path, open a new CMD window (you must reopen a CMD window for enviornment variable changes to take effect) and type in a gam command, or just "gam" is fine. If you see output, you're good.

If not, try a reboot. If it still doesn't work, open an issue.
