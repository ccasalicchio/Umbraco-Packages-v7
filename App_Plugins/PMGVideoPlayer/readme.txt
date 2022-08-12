This data type will allow an editor to paste in a youtube or vimeo URL and it will show a player with the video in the backend.  Right now it only supports vimeo and youtube.

After trying a few video players, I settled on videojs however I had to modify a few of the plugins that display the youtube and vimeo because they weren't working with the newest version of videojs.

Notes:

Only supports the html player for now.

You must touch the web.config/flush cache after installing.

Credits:

Videojs: https://github.com/videojs/video.js

Youtube Plugin: https://github.com/eXon/videojs-youtube

Vimeo Plugin: https://github.com/eXon/videojs-vimeo