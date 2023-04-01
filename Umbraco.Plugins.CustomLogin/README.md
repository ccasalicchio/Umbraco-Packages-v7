# Custom Login

Plugin for Umbraco 7.4.3

This is a tweak for the login page, changing the visual of the layout. It adds a custom css and javascript injections into the Login page.

It also adds a "loading" after clicking login, and it changes the background image for the screen.

To customize it, you should open ~/App_Plugins/Custom.Login.Page/css/style.css and change the style to fit your needs.

#### Umbraco 7.6+

The login background can be changed via config file, so this plugin is not necessary

`~/config/UmbracoSettings.config`

		<content><loginBackgroundImage>/Assets/images/logo.png</loginBackgroundImage></content> 

