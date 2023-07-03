var uri = "https://localhost:7027/api/TodoItems/Login"

function check() {
    if (document.getElementById('remember').checked) {
        console.log("checked")
    }
    else {
        console.log("Not checked")
    }
}
function dark() {
    var darkmode = document.getElementById("darkmode");
    var filter = document.getElementById("filter");
    var isDark = false;
    document.body.classList.toggle("dark-theme");
    filter.classList.toggle("show");
    isDark = !isDark;
    if (isDark) {
        darkmode.innerHTML = "Dark Mode";
        var fill = document.getElementsByClassName("filter1");
        fill.classList.remove("hidden");
        fill.classList.add("filter");

    }
    else {
        darkmode.innerHTML = "Light Mode";
    }
}
function example() {
    const user = document.getElementById('userid');
    const pass = document.getElementById('pass');

    const item = {
        username: user.value.trim(),
        password: pass.value.trim()
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(response => {
            sessionStorage.setItem("token", response)
            if (response != 'User Not Found') {
                window.location.href = 'https://localhost:7290'
            }
            else {
                alert('User Not Found')
            }
        })
}
