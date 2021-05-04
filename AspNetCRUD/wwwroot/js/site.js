let hamburguerMenu = document.querySelector('.hamburguer-menu')
let menu = document.querySelector('.menu')
let closeMenuSign = document.querySelector('.close-menu')
let lines = document.querySelector('.lines')

hamburguerMenu.addEventListener('click', () => {
    menu.classList.toggle('display-block')
    lines.classList.toggle('display-none')
    closeMenuSign.classList.toggle('display-block')
})