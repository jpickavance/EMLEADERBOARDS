mergeInto(LibraryManager.library,
{
    PostTrialData: function (tableName, username, trialNum, trialWPM)
    {
        var params =
        {
            TableName: Pointer_stringify(tableName),
            Item:
            { // left hand side must be whats in the dynamo DB table
                "tokenId": Pointer_stringify(username), // primary key jkhkjh
                "trial": Pointer_stringify(trialNum), // sort key
                "trialWPM": Pointer_stringify(trialWPM)
            }
        };
        var awsConfig =
        {
            region: "eu-west-2",
            endpoint: "https://dynamodb.eu-west-2.amazonaws.com",
            accessKeyId: "YOUR ACCESS ID",
            secretAccessKey: "YOUR SECRET KEY"
        };
        AWS.config.update(awsConfig);
        var docClient = new AWS.DynamoDB.DocumentClient();
        var returnStr = "Error";
        docClient.put(params, function(err, data) // request to the server function is call-back fn
        { // if this was a docClient.get() - if that data is available, can then play the game (e.g. checking tokens)
            if (err)
            {
                returnStr = "Error:" + JSON.stringify(err, undefined, 2);
                SendMessage('ExperimentController', 'StringCallback', returnStr);
            }
            else
            {
                returnStr = "Data Inserted:" + JSON.stringify(data, undefined, 2);
                SendMessage('ExperimentController', 'StringCallback', returnStr);
            }
        });
    },
    PostUserData: function (tableName, username, age, location, widthPx, heightPx, pxRatio, browserVersion,
                             clickedInfo, consentProvided, consentTime, startTime)
    {
        var params =
        {
            TableName: Pointer_stringify(tableName),
            Item:
            {
                "tokenId": Pointer_stringify(username),
                "age": Pointer_stringify(age),
                "location": Pointer_stringify(location),
                "widthPx": Pointer_stringify(widthPx),
                "heightPx": Pointer_stringify(heightPx),
                "pxRatio": Pointer_stringify(pxRatio),
                "browserVersion": Pointer_stringify(browserVersion),
                "clickedInfo": Pointer_stringify(clickedInfo),
                "consentProvided": Pointer_stringify(consentProvided),
                "consentTime": Pointer_stringify(consentTime),
                "startTime": Pointer_stringify(startTime)

                // also want:
                // if buttons are clicked on the end-screen
                // if it was submitted to the database?
                // what they say on the feedback form
            }
        };
        var awsConfig =
        {
            region: "eu-west-2",
            endpoint: "https://dynamodb.eu-west-2.amazonaws.com",
            accessKeyId: "YOUR ACCESS ID",
            secretAccessKey: "YOUR SECRET KEY"
        };
        AWS.config.update(awsConfig);
        var docClient = new AWS.DynamoDB.DocumentClient();
        var returnStr = "Error";
        docClient.put(params, function(err, data)
        {
            if (err)
            {
                returnStr = "Error:" + JSON.stringify(err, undefined, 2);
                SendMessage('ExperimentController', 'StringCallback', returnStr);
            }
            else
            {
                returnStr = "Data Inserted:" + JSON.stringify(data, undefined, 2);
                SendMessage('ExperimentController', 'StringCallback', returnStr);
            }
        });
    },
    getScreenWidth: function()
    {
        var widthPx = String(screen.width);
        var bufferSize = lengthBytesUTF8(widthPx) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(widthPx, buffer, bufferSize);
        return buffer;
    },
    getScreenHeight: function()
    {
        var heightPx = String(screen.height);
        var bufferSize = lengthBytesUTF8(heightPx) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(heightPx, buffer, bufferSize);
        return buffer;
    },
    getPixelRatio: function()
    {
        var pxRatio = String(window.devicePixelRatio);
        var bufferSize = lengthBytesUTF8(pxRatio) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(pxRatio, buffer, bufferSize);
        return buffer;
    },
    getBrowserVersion: function()
    {
        function get_browser() 
        {
            var ua=navigator.userAgent,tem,M=ua.match(/(opera|chrome|safari|firefox|msie|trident(?=\/))\/?\s*(\d+)/i) || []; 
            if(/trident/i.test(M[1])){
                tem=/\brv[ :]+(\d+)/g.exec(ua) || []; 
                return {name:'IE',version:(tem[1]||'')};
                }   
            if(M[1]==='Chrome'){
                tem=ua.match(/\bOPR|Edge\/(\d+)/)
                if(tem!=null)   {return {name:'Opera', version:tem[1]};}
                }   
            M=M[2]? [M[1], M[2]]: [navigator.appName, navigator.appVersion, '-?'];
            if((tem=ua.match(/version\/(\d+)/i))!=null) {M.splice(1,1,tem[1]);}
            return {
            name: M[0],
            version: M[1]}
        };
        var browser = get_browser();
        var browserName = String(browser.name);
        var version = String(browser.version);
        var browserVersion = browserName + ", " + version;
        var bufferSize = lengthBytesUTF8(browserVersion) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(browserVersion, buffer, bufferSize);
        return buffer;
    },
    fullscreenMenuListener: function()
    {
        if(document.addEventListener)
        {
            document.addEventListener('fullscreenchange', exitHandler3, false);
            document.addEventListener('mozfullscreenchange', exitHandler3, false);
            document.addEventListener('MSFullscreenChange', exitHandler3, false);
            document.addEventListener('webkitfullscreenchange', exitHandler3, false);
            
        }
        function exitHandler3()
        {
            if (document.webkitIsFullScreen || document.mozFullScreen || (document.msFullscreenElement !== null && document.msFullscreenElement !== undefined))
            {
                SendMessage('MainMenu', 'ToggleFullscreen', 'fullscreen');
            }
            else if (!document.webkitIsFullScreen || !document.mozFullScreen || (document.msFullscreenElement == null && document.msFullscreenElement !== undefined))
            {
                SendMessage('MainMenu', 'ToggleFullscreen', 'exitFullscreen');
            }
        }
    },
    fullscreenListener: function() //could collapse this into function above if I can get the unity scene from js
    {
        if(document.addEventListener)
        {
            document.addEventListener('fullscreenchange', exitHandler, false);
            document.addEventListener('mozfullscreenchange', exitHandler, false);
            document.addEventListener('MSFullscreenChange', exitHandler, false);
            document.addEventListener('webkitfullscreenchange', exitHandler, false);
            
        }
        function exitHandler()
        {
            if (document.webkitIsFullScreen || document.mozFullScreen || (document.msFullscreenElement !== null && document.msFullscreenElement !== undefined))
            {
                SendMessage('Pause', 'ToggleFullscreen', 'fullscreen');
            }
            else if (!document.webkitIsFullScreen || !document.mozFullScreen || (document.msFullscreenElement == null && document.msFullscreenElement !== undefined))
            {
                SendMessage('Pause', 'ToggleFullscreen', 'exitFullscreen');
            }
        }
    },
    InsertLeaderboardUser: function (tableName, username, aveWPM, age, county1)
    {
        var params =
        {
            TableName: Pointer_stringify(tableName),
            Item:
            {
                "tokenId": Pointer_stringify(username),
                "aveWPM": Pointer_stringify(aveWPM),
                "age": Pointer_stringify(age),
                "county1": Pointer_stringify(county1)
            }
        };
        var awsConfig =
        {
            region: "eu-west-2",
            endpoint: "https://dynamodb.eu-west-2.amazonaws.com",
            accessKeyId: "YOUR ACCESS ID",
            secretAccessKey: "YOUR SECRET KEY"
        };
        AWS.config.update(awsConfig);
        var docClient = new AWS.DynamoDB.DocumentClient();
        var returnStr = "Error";
        docClient.put(params, function(err, data)
        {
            if (err)
            {
                returnStr = "Error:" + JSON.stringify(err, undefined, 2);
                SendMessage('ExperimentController', 'StringCallback', returnStr);
            }
            else
            {
                returnStr = "Data Inserted:" + JSON.stringify(data, undefined, 2);
                SendMessage('ExperimentController', 'StringCallback', returnStr);
            }
        });
    },
    UpdateUser: function (tableName, username, aveWPM, percentile)
    {
        var params =
        {
            TableName: Pointer_stringify(tableName),
            Key:
            {
                "tokenId": Pointer_stringify(username)
            },
            ////// do these expressions below come from the database?
            //UpdateExpression: "set n_pauses = :n, trials = :t",
            UpdateExpression: "set aveWPM = :a, percentile = :p",
            ExpressionAttributeValues:
            {
                ":a": Pointer_stringify(aveWPM),
                ":p": Pointer_stringify(percentile)

                // also want:
                // if buttons are clicked on the end-screen
                // what they say on the feedback form
            },
            ReturnValues:"UPDATED_NEW"
        };
        var awsConfig =
        {
            region: "eu-west-2",
            endpoint: "https://dynamodb.eu-west-2.amazonaws.com",
            accessKeyId: "YOUR ACCESS ID",
            secretAccessKey: "YOUR SECRET KEY"
        };
        AWS.config.update(awsConfig);
        var docClient = new AWS.DynamoDB.DocumentClient();
        docClient.update(params, function(err, data)
        {
            if (err)
            {
                console.log(err);
            }
            else if (data )
            {
                console.log("summary data added to user table");
            }
        });
    },
    GetLeaderboardSize: function (tableName)
    {
        var params =
        {
            TableName: Pointer_stringify(tableName)
        };
        var awsConfig =
        {
            region: "eu-west-2",
            endpoint: "https://dynamodb.eu-west-2.amazonaws.com",
            accessKeyId: "YOUR ACCESS ID",
            secretAccessKey: "YOUR SECRET KEY"
        };
        AWS.config.update(awsConfig);
        var dynamodb = new AWS.DynamoDB();
        dynamodb.describeTable(params, function(err, data)
        {
            if (err)
            {
                var returnStr = "Unable to retrieve table length";
                SendMessage('Leaderboard', 'ErrorCallback', returnStr);
            }
            else
            {
                console.log(data.Table);
                SendMessage('Leaderboard', 'setLeaderboardSize', data.Table.ItemCount);
            }
        });
    },
    ReadLeaderboardTop10: function (tableName)
    {
        var params =
        {
            TableName: Pointer_stringify(tableName),
            ProjectionExpression: "tokenId, age, aveWPM, county1"
        };
        var awsConfig =
        {
            region: "eu-west-2",
            endpoint: "https://dynamodb.eu-west-2.amazonaws.com",
            accessKeyId: "YOUR ACCESS ID",
            secretAccessKey: "YOUR SECRET KEY"
        };
        AWS.config.update(awsConfig);
        var docClient = new AWS.DynamoDB.DocumentClient();
        docClient.scan(params, function(err, data)
        {
            if (err)
            {
                var returnStr = "Unable to retrieve items";
                console.log(err);
                SendMessage('Leaderboard', 'ErrorCallback', returnStr);
            }
            else
            {
                console.log("success", data.Items);
                data.Items.forEach(function(element)
                    {
                        var itemString = JSON.stringify(element);
                        SendMessage('Leaderboard', 'appendResult', itemString);
                    });
            }
        });
    },
    OpenWindow: function(link)
    {
        var url = Pointer_stringify(link);
        document.onmouseup = function()
        {
            window.open(url);
            document.onmouseup = null;
        }
    },
});