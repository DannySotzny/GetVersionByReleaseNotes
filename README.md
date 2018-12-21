# Get Version by Release Notes

sometimes it will be helply to get the VersionNumber by your Documentation.. 
and so this Tool analyse your RELEASE NOTES File to detect your Version.

The Structure is really simple:

Everytime you release a Major or Minor Release you will enter a Headline e.g.

```
# 1.0 - Inital Version
````
The hyphen between version number and description is important here!
and for each task or correction there is a new point that has to be started with a minus


## Example:
```
# 1.1 - New Feature ABC
  - Hotfix
  - One Version per line 
  - and between version and description must be a minus!
# 1.0 - Initial Version
```

Here it will output version `1.1.3` (from last Headline) and 3 subitems.
If there is no subitem you´ll get `1.1.0`.

The fourth digit of the version number is reserved for the build number

### Get Started

    > dotnet run examples/ReleaseNotes.md
    1.1.3
    
    > dotnet run examples/ReleaseNotes2.md
    1.1.0