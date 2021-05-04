pagination = {
    totalItems: 0,
    pageSize: 0,
    currentPage: 0,
    paramName: '',
    maxNumberOfPages: 0,
    totalNumberOfLinks: 0,

    interface: {
        visual: '',
        previousLink: '',
        nextLink: '',
        beginLink: '',
        endLink: ''
    },

    getTotalNumberOfLinks() {
        return Math.ceil(this.totalItems / this.pageSize)
    },

    getCurrentUrl() {
        return window.location.href
    },

    generateBeginEndLinks(url, position) {
        // Begin Link
        if (this.currentPage != 1) {
            this.interface.visual.insertBefore(this.interface.beginLink, pagination.interface.visual.children[0])
            let newParamAndValue = `${this.paramName}=${1}`
            if (position != -1) {
                let paramAndValue = url.slice(position).split('&')[0]
                this.interface.beginLink.href = url.replace(paramAndValue, newParamAndValue)
            }
            else {
                this.interface.beginLink.href = url + newParamAndValue
            }
            this.interface.beginLink.classList.add('previous-link')
        }

        // End Link
        if (this.currentPage != this.totalNumberOfLinks) {
            this.interface.visual.appendChild(this.interface.endLink)
            let newParamAndValue = `${this.paramName}=${this.totalNumberOfLinks}`
            if (position != -1) {
                let paramAndValue = url.slice(position).split('&')[0]
                this.interface.endLink.href = url.replace(paramAndValue, newParamAndValue)
            }
            else {
                this.interface.endLink.href = url + newParamAndValue
            }
            this.interface.endLink.classList.add('next-link')
        }
    },

    generatePreviosNextLinks(url, position) {
        // Previous Link
        if (this.currentPage != 1) {
            this.interface.visual.insertBefore(this.interface.previousLink, pagination.interface.visual.children[0])
            let newParamAndValue = `${this.paramName}=${this.currentPage - 1}`
            if (position != -1) {
                let paramAndValue = url.slice(position).split('&')[0]
                this.interface.previousLink.href = url.replace(paramAndValue, newParamAndValue)
            }
            else {
                this.interface.previousLink.href = url + newParamAndValue
            }
            this.interface.previousLink.classList.add('previous-link')
        }

        // Next Link
        if (this.currentPage != this.totalNumberOfLinks) {
            this.interface.visual.appendChild(this.interface.nextLink)
            let newParamAndValue = `${this.paramName}=${this.currentPage + 1}`
            if (position != -1) {
                let paramAndValue = url.slice(position).split('&')[0]
                this.interface.nextLink.href = url.replace(paramAndValue, newParamAndValue)
            }
            else {
                this.interface.nextLink.href = url + newParamAndValue
            }
            this.interface.nextLink.classList.add('next-link')
        }
    },

    generateLinks() {
        let url = this.getCurrentUrl()

        let position = url.search(this.paramName + '=')
        // If it already has a position it means that I just have to change the values
        if (position != -1) {
            let paramAndValue = url.slice(position).split('&')[0]
            // The current page will be the value after the "=" character, example: "page=5", 5 is the current page
            this.currentPage = parseInt(paramAndValue.split('=')[1])

            /*
            I'm making that the current page stay on the middle of the links, so I need to make the start of the iteration the
            current page minus half of the max number of links per page.
            */
            let i
            let iterations
            if (this.totalNumberOfLinks <= this.maxNumberOfPages) {
                i = 1
                iterations = this.totalNumberOfLinks
            }
            else {
                let halfMaxNumberOfPages = Math.floor(this.maxNumberOfPages / 2)
                /* 
                If the current page plus the rest of the links that should be added, half of the max number of links per page, is bigger
                than the total number of links, it means that I can't add more links
                */
                if ((this.currentPage + halfMaxNumberOfPages) >= this.totalNumberOfLinks) {
                    i = this.currentPage - halfMaxNumberOfPages - ((this.currentPage + halfMaxNumberOfPages) - this.totalNumberOfLinks)
                }
                else if (this.currentPage > halfMaxNumberOfPages) {
                    i = this.currentPage - halfMaxNumberOfPages
                }
                else {
                    i = 1
                }

                iterations = i + this.maxNumberOfPages
            }

            // Generating the links
            for (i; i <= iterations; i++) {
                let linkElement = document.createElement('a')
                let newParamAndValue = `${this.paramName}=${i}`
                linkElement.href = url.replace(paramAndValue, newParamAndValue)

                if (i == this.currentPage)
                    linkElement.classList.add('current-page')

                this.interface.visual.appendChild(linkElement)
                linkElement.innerText = i
            }
        }
        else {
            let withoutQueryString = url.indexOf('?') == -1
            if (withoutQueryString) url += '?'

            let i = 1
            let iterations = (this.totalNumberOfLinks < this.maxNumberOfPages) ? this.totalNumberOfLinks : this.maxNumberOfPages
            // Generating the links
            for (i = 1; i <= iterations; i++) {
                let linkElement = document.createElement('a')
                linkElement.href = url
                linkElement.href += withoutQueryString ? `${this.paramName}=${i}` : `&${this.paramName}=${i}`
                this.interface.visual.appendChild(linkElement)
                linkElement.innerText = i
            }

            this.interface.visual.children[0].classList.add('current-page')
        }

        this.generateBeginEndLinks(url, position)
        this.generatePreviosNextLinks(url, position)
    },

    setValues(totalItems, pageSize, maxNumberOfPages, paramName, previousLinkText, nextLinkText, beginLinkText, endLinkText) {
        this.currentPage = 1

        this.totalItems = totalItems
        this.pageSize = pageSize
        this.paramName = paramName
        this.maxNumberOfPages = maxNumberOfPages - 1
        this.totalNumberOfLinks = this.getTotalNumberOfLinks()

        let previousLink = document.createElement('a')
        previousLink.classList.add('previous-link')
        previousLink.innerText = previousLinkText
        this.interface.previousLink = previousLink

        let nextLink = document.createElement('a')
        nextLink.classList.add('next-link')
        nextLink.innerText = nextLinkText
        this.interface.nextLink = nextLink

        let beginLink = document.createElement('a')
        beginLink.classList.add('previous-link')
        beginLink.innerText = beginLinkText
        this.interface.beginLink = beginLink

        let endLink = document.createElement('a')
        endLink.classList.add('next-link')
        endLink.innerText = endLinkText
        this.interface.endLink = endLink

        this.interface.visual = document.querySelector('.pagination')
    },

    paginate(totalItems, pageSize, maxNumberOfPages, paramName, previousLinkText = '<', nextLinkText = '>', beginLinkText = '<<', endLinkText = '>>') {
        this.setValues(totalItems, pageSize, maxNumberOfPages, paramName, previousLinkText, nextLinkText, beginLinkText, endLinkText)
        this.generateLinks()
    }
}