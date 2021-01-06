pagination = {
    totalItems: 0,
    pageSize: 0,
    currentPage: 1,
    visual: document.querySelector('.pagination'),
    paramName: '',

    getNumberOfLinks() {
        return Math.ceil(this.totalItems / this.pageSize)
    },

    getCurrentUrl() {
        console.log('Current url =', window.location.href)
        return window.location.href
    },

    generateLinks() {
        let url = this.getCurrentUrl()
        let numberOfLinks = this.getNumberOfLinks()

        let pos = url.search(this.paramName + '=')
        if (pos != -1) {
            let paramAndValue = url.slice(pos).split('&')[0]
            this.currentPage = parseInt(paramAndValue.split('=')[1])
            console.log('current page =', this.currentPage)

            for (let i = 1; i <= numberOfLinks; i++) {
                let linkElement = document.createElement('a')
                let newParamAndValue = `${this.paramName}=${i}`
                linkElement.href = url.replace(paramAndValue, newParamAndValue)
                if (i == this.currentPage)
                    linkElement.classList.add('current-page')
                this.visual.appendChild(linkElement)
                linkElement.innerText = i
            }
        }
        else {
            let withoutQueryString = url.indexOf('?') == -1
            if (withoutQueryString)
                url += '?'

            for (let i = 1; i <= numberOfLinks; i++) {
                let linkElement = document.createElement('a')
                linkElement.href = url
                linkElement.href += withoutQueryString ? `${this.paramName}=${i}` : `&${this.paramName}=${i}`
                this.visual.appendChild(linkElement)
                linkElement.innerText = i
            }
            this.visual.children[0].classList.add('current-page')
        }
    },

    setValues(totalItems, pageSize, paramName) {
        this.totalItems = totalItems
        this.pageSize = pageSize
        this.paramName = paramName
    },

    paginate(totalItems, pageSize, paramName) {
        this.currentPage = 1
        this.setValues(totalItems, pageSize, paramName)
        this.generateLinks()
    }
}