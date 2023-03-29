# Simple Analytics

_former visit counter_

##### Umbraco v7.15.10

- Simple analytics to keep track of visits within the site (for page counters and such)

### Simple Analytics Script

- A js script to track the visits of your site, grabs the node Id, the public IP address, and saves it to the database
- Connects to [https://api.ipify.org?format=text](https://api.ipify.org?format=text) to get the public IP address. You are free to change it.
- Uses the Lite version of the IP2Location BIN [https://lite.ip2location.com](https://lite.ip2location.com) to get location information
- Comes with a basic front-end example

### Simple Analytics Dashboard

- Installs a Dashboard to both Content and Settings, to display a summary of the analytics information
- Displays Browser User Agent, Location, IP, and Duration of Visits
- Lists entry and exit Urls
- Basic widgets with useful information

#### Screenshots

![Imgur](https://i.imgur.com/ZkXCWlA.png)
![Imgur](https://i.imgur.com/k5CDLSJ.png)
![Imgur](https://i.imgur.com/k5CDLSJ.png)
![Imgur](https://i.imgur.com/jWLByaN.png)
 
Visit the [Project Page](https://our.umbraco.org/projects/backoffice-extensions/visit-counter/) in the Umbraco Community

Install via Nuget

		Install-Package SplatDev.Umbraco.Plugins.SimpleAnalytics

##### Specs
- Value Type: STRING
- Creates a new Table `Analitics Visits`
 
##### Included Files:
- LICENSE
- README.md
- Components
- Components\SimpleAnalyticsComponent.cs
- Controllers
- Controllers\AnalyticsApiController.cs
- Dashboards
- Dashboards\AnalyticsDashboard.cs
- Extensions
- Extensions\BrowserExtensions.cs
- Extensions\Ip2LocationExtensions.cs
- Models
- Models\AnalyticsVisit.cs
- Models\AnalyticsVisitModel.cs
- Models\BrandVersion.cs
- Models\BrowserInfo.cs
- Models\IPMapping.cs
- Models\PagedResults.cs
- Models\VisitFilter.cs
- Models\VisitStats.cs
- scripts
- scripts\analytics.js
- Views
- Views\Partials
- Views\Web.config
- Views\Example-Analytics.cshtml
- Views\Partials\_analytics.cshtml
- App_Plugins
- App_Plugins\VisitCounter
- App_Plugins\VisitCounterDashboard
- App_Plugins\VisitCounter\css
- App_Plugins\VisitCounter\js
- App_Plugins\VisitCounter\views
- App_Plugins\VisitCounter\package.manifest
- App_Plugins\VisitCounter\css\style.css
- App_Plugins\VisitCounter\js\controller.js
- App_Plugins\VisitCounter\js\controller-original.js
- App_Plugins\VisitCounter\views\view.html
- App_Plugins\VisitCounterDashboard\assets
- App_Plugins\VisitCounterDashboard\css
- App_Plugins\VisitCounterDashboard\js
- App_Plugins\VisitCounterDashboard\lang
- App_Plugins\VisitCounterDashboard\views
- App_Plugins\VisitCounterDashboard\IP2LOCATION-LITE-DB11.BIN
- App_Plugins\VisitCounterDashboard\package.manifest
- App_Plugins\VisitCounterDashboard\Web.config
- App_Plugins\VisitCounterDashboard\assets\browsers
- App_Plugins\VisitCounterDashboard\assets\flags
- App_Plugins\VisitCounterDashboard\assets\os
- App_Plugins\VisitCounterDashboard\assets\globe.png
- App_Plugins\VisitCounterDashboard\assets\pin.png
- App_Plugins\VisitCounterDashboard\assets\browsers\*
- App_Plugins\VisitCounterDashboard\assets\flags\*.png
- App_Plugins\VisitCounterDashboard\assets\os\*.png
- App_Plugins\VisitCounterDashboard\css\style.css
- App_Plugins\VisitCounterDashboard\css\style.min.css
- App_Plugins\VisitCounterDashboard\css\style.scss
- App_Plugins\VisitCounterDashboard\js\angular-chart.js
- App_Plugins\VisitCounterDashboard\js\angular-chart.min.js
- App_Plugins\VisitCounterDashboard\js\angular-chart.min.js.map
- App_Plugins\VisitCounterDashboard\js\angular-merge.js
- App_Plugins\VisitCounterDashboard\js\chart.js
- App_Plugins\VisitCounterDashboard\js\Chart.min.js
- App_Plugins\VisitCounterDashboard\js\controller.js
- App_Plugins\VisitCounterDashboard\lang\en-US.xml
- App_Plugins\VisitCounterDashboard\views\view.html

[Feedback](mailto:feedback@splatdev.com) is appreciated
