document.addEventListener('DOMContentLoaded', () => {
    const xhttp = new XMLHttpRequest();
    let ip = '';

    addEventListener("beforeunload", () => {
        let exitUrl = `${window.location.pathname}`;
        logExit(exitUrl);
    });

    async function getVisitCount(nodeId) {
        const getIpUrl = `/Umbraco/Analytics/AnalyticsApi/GetVisitCount?nodeId=${nodeId}`;
        xhttp.open("GET", getIpUrl, true);
        xhttp.send();
        xhttp.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                let visitCount = +xhttp.responseText;
                document.getElementById('visit-count').value = visitCount;
                console.log('you visited this page:', visitCount, 'x');
            }
        };
    }

    async function getIP() {
        const getIpUrl = 'https://api.ipify.org?format=text';
        xhttp.open("GET", getIpUrl, true);
        xhttp.send();
        xhttp.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                ip = xhttp.responseText === '' ? '0.0.0.0' : xhttp.responseText;
                console.log('your public IP:', ip);
                logVisit();
            }
        };
    }
    async function logExit(exitUrl) {
        const logVisitUrl = "/Umbraco/Analytics/AnalyticsApi/LogVisit";
        let id = document.querySelector("[data-node]").getAttribute("data-node");
        let thisVisit = {
            NodeId: id,
            IPAddress: ip,
            ExitUrl: exitUrl ?? ''
        }

        xhttp.open("GET", `${logVisitUrl}?jsonData=${JSON.stringify(thisVisit)}`, true);
        xhttp.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
        xhttp.send();
        xhttp.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                console.log('logged', xhttp.responseText);
            }
        };
    }

    async function logVisit(exitUrl) {
        const logVisitUrl = "/Umbraco/Analytics/AnalyticsApi/LogVisit";
        let id = document.querySelector("[data-node]").getAttribute("data-node");

        let thisVisit = {
            Id: 0,
            NodeId: id,
            IPAddress: ip,
            Browser: null,
            Resolution: `${window.screen.availWidth}x${window.screen.availHeight}`,
            ExitUrl: exitUrl ?? '',
            RecurringVisit: false
        }

        console.log(navigator.userAgent);

        if (navigator.userAgent !== undefined) {
            thisVisit.Browser = {
                appVersion: navigator.appVersion,
                language: navigator.language,
                platform: navigator.platform,
                os: navigator.userAgentData ? navigator.userAgentData.platform : navigator.oscpu,
                userAgent: navigator.userAgent,
                versions: navigator.userAgentData ? navigator.userAgentData.brands : [],
                vendor: navigator.vendor
            }
        }

        setTimeout(() => {
            xhttp.open("GET", `${logVisitUrl}?jsonData=${JSON.stringify(thisVisit)}`, true);
            xhttp.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
            xhttp.send();
            xhttp.onreadystatechange = function () {
                if (this.readyState == 4 && this.status == 200) {
                    console.log('logged', xhttp.responseText);
                }
            };
        }, 1 * 1000);
        await getVisitCount(thisVisit.NodeId);
    }


    getIP();
})
