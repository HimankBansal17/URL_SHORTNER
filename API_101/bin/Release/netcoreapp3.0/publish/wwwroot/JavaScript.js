//variable stroing address to the 
const uri = 'APIModels';
var Url;
let urls = [];
var count;
var longurl;
var shorturl
var token;
var DBUrls;
var status;
var response;

//fucntion to get all the urls in the databse and store a copy of data base in 
function getUrls() {
    fetch(uri)
        .then(response => console.log(response.status, response.json().then(data => _displayUrls(data))))
        .catch(error => console.error('Unable to get Urls', error));
}



//To Log the status code and messagae returned from the API for the 
function status(s) {
    response = s;
    if (response == 200) {
        return console.info("HTTP STATUS: " + response + " The Url entered already exists in the database");
    }
    else if (response == 201) {
        return console.info("HTTP STATUS: " + response + " The Short Url Has been Created");

    }
    else if (response == 400)
        return console.error("HTTP STATUS: " + response + " The Url Entered Is not Valid")
}


//function to start the API call for generating the short url
function AssignCode() {
    const addNameTextbox = document.getElementById('add-url');
    var n = addNameTextbox.value.trim();

        //make an instance of data models to upload to the API server
        Url = {
            LongUrl: addNameTextbox.value.trim(),
        };
        addUrl();
}

//function to make a post request to the API
function addUrl() {
    const addNameTextbox = document.getElementById('add-url');
        fetch(uri, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(Url)// to conver the data into json format
        })
            .then(response => console.log(status(response.status, response.json().then(data => {
                _displaynewurl(data);//display only the short url for entered url
                console.log(data);//log the response in the console
                addNameTextbox.value = '';//cleare the value in the input box
            })
                
            

            )))
            .catch(error => console.error('Unable to add Url' + error, error));// logs any error found when the request is made
    }

//function to display only  the new gerenated short url 
function _displaynewurl(data) {
    

    var urlnewbox = document.getElementById('newurl');// get the element from html
    urlnewbox.innerHTML = data.shortUrl;//set the short url value as the text value in the element
    const tBody = document.getElementById('urls');
    tBody.innerHTML = '';
    let tr = tBody.insertRow();

    let td2 = tr.insertCell(0);
    let nameNode = document.createTextNode(data.shortUrl);
    td2.appendChild(nameNode);

    let td5 = tr.insertCell(1);
    let ourl = document.createTextNode(data.longUrl);
    td5.appendChild(ourl);
}

    

//display all the urls in the data base when the user presses the button display All urls
function _displayUrls(data) {
    const tBody = document.getElementById('urls');
    tBody.innerHTML = '';

    const button = document.createElement('button');

    data.forEach(Url => {
        let tr = tBody.insertRow();

        let td2 = tr.insertCell(0);
        let nameNode = document.createTextNode(Url.shortUrl);
        td2.appendChild(nameNode);

        let td5 = tr.insertCell(1);
        let ourl = document.createTextNode(Url.longUrl);
        td5.appendChild(ourl);
    });

    urls = data;   
}