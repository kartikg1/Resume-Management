/// <reference path="jquery-3.3.1.min.js" />

function getAccessToken() {
    if (location.hash) {
        if (location.hash.split('access_token=')) {
            var accessToken = location.hash.split('access_token=')[1].split('&')[0];
            if (accessToken) {
                isUserRegistered(accessToken);
            }
        }
    }
}

function isUserRegistered(accessToken) {
    $.ajax({
        url: 'https://localhost:44357/api/Account/UserInfo',
        method: 'GET',  
        headers: {
            'content-type': 'application/JSON',
            'Authorization': 'Bearer ' + accessToken
        },
        success: function (response) {
            if (response.HasRegistered) {
                localStorage.setItem('accessToken', accessToken);
                localStorage.setItem('userName', response.Email);
                window.location.href = "https://localhost:4200/archived";
            }
            else {
                signupExternalUser(accessToken);
            }
        }
    });
}


function signupExternalUser(accessToken) {
    $.ajax({
        url: 'https://localhost:44357/api/Account/RegisterExternal',
        method: 'POST',
        headers: {
            'content-type': 'application/json',
            'Authorization': 'Bearer ' + accessToken
        },
        success: function () {
            window.location.href = "https://localhost:44357/api/Account/ExternalLogin?provider=Google&response_type=token&client_id=self&redirect_uri=https%3A%2F%2Flocalhost%3A4200%2Fview&state=oIHzYNikreT1WZ05yEvNqSpHARsywtU7bQyUUb73au01";
        }
    });

}