const cancel = document.querySelector(".cancel")
const deletebutton = document.querySelector(".deletebutton")
const warning = document.querySelector(".warningheader")
const warningproj = document.querySelector(".warningheaderproj")
function popwarning(){
    if (warning.style.display = "none") {
        warning.style.display = "flex";
    }
}

function exit(){
    if (warning.style.display = "flex"){
        warning.style.display = "none";
    }
}

function popwarningproj(){
    if (warningproj.style.display = "none") {
        warningproj.style.display = "flex";
    }
}

function exitproj(){
    if (warningproj.style.display = "flex"){
        warningproj.style.display = "none";
    }
}
