(function () {
    var app = angular.module('gogol', ['ngMaterial', 'ngMessages', 'ngAnimate', 'infinite-scroll']);


    app.controller('NavbarController', function ($scope, $rootScope) {

    });

    app.controller('FilterController', function ($scope, $timeout, $mdSidenav, $rootScope,
        authorFilters, categoryFilters, publisherFilters, filterBooks) {

        $scope.toggleLeft = buildToggler('left');
        $scope.toggleRight = buildToggler('right');

        function buildToggler(componentId) {
            return function () {
                $mdSidenav(componentId).toggle();
            }
        };

        $scope.categories = [];
        $scope.authors = [];
        $scope.publishers = [];
        $scope.selected = [1];

        $scope.setFilters = function () {
            authorFilters.query()
                .then(function (data) {
                    for (var i = 0; i < data.length; i++) {
                        it = {};
                        it.string = data[i]
                        it.check = false;
                        $scope.authors.push(it);
                    }
                })
            categoryFilters.query()
                .then(function (data) {
                    for (var i = 0; i < data.length; i++) {
                        it = {};
                        it.string = data[i]
                        it.check = false;
                        $scope.categories.push(it);
                    }
                })
            publisherFilters.query()
                .then(function (data) {
                    for (var i = 0; i < data.length; i++) {
                        it = {};
                        it.string = data[i]
                        it.check = false;
                        $scope.publishers.push(it);
                    }
                })
        }

        $scope.setFilters();

        $scope.filterRequestBody = {
            authors: [],
            categories: [],
            publishers: []
        }

        $scope.addFilterItem = function (item, section) {
            if (section === "author") {
                if (!item.check) {
                    $scope.filterRequestBody.authors.push(item.string);
                } else {
                    for (var index = 0; index < $scope.filterRequestBody.authors.length; index++) {
                        if ($scope.filterRequestBody.authors[index] === item.string) {
                            $scope.filterRequestBody.authors.splice(index, 1);
                        }
                    }
                }

            }
            else if (section === "publisher") {
                $scope.filterRequestBody.publishers.push(item.string);
                if (!item.check) {
                    $scope.filterRequestBody.publishers.push(item.string);
                } else {
                    for (var index = 0; index < $scope.filterRequestBody.publishers.length; index++) {
                        if ($scope.filterRequestBody.publishers[index] === item.string) {
                            $scope.filterRequestBody.publishers.splice(index, 1);
                        }
                    }
                }
            }
            else if (section === "category") {
                if (!item.check) {
                    $scope.filterRequestBody.categories.push(item.string);
                } else {
                    for (var index = 0; index < $scope.filterRequestBody.categories.length; index++) {
                        if ($scope.filterRequestBody.categories[index] === item.string) {
                            $scope.filterRequestBody.categories.splice(index, 1);
                        }
                    }
                }
            }
        }

        $scope.filter = function () {
            filterBooks.query(JSON.stringify($scope.filterRequestBody))
                .then(function (data) {
                    $rootScope.$emit('Refresh', data);
                })
        }

        $scope.toggle = function (item, list, check, section) {
            var idx = list.indexOf(item);
            if (idx > -1) {
                list.splice(idx, 1);
            }
            else {
                list.push(item);
            }

            $scope.addFilterItem(item, section);
        };

        $scope.exists = function (item, list) {
            return list.indexOf(item) > -1;
        };

        $scope.isIndeterminate = function () {
            return ($scope.selected.length !== 0 &&
                $scope.selected.length !== $scope.items.length);
        };

        $scope.isChecked = function () {
            return $scope.selected.length === $scope.items.length;
        };

        $scope.toggleAll = function () {
            if ($scope.selected.length === $scope.items.length) {
                $scope.selected = [];
            } else if ($scope.selected.length === 0 || $scope.selected.length > 0) {
                $scope.selected = $scope.items.slice(0);
            }
        };
    });

    app.controller("SearchController", function ($timeout, $q) {

        var self = this;

        // list of `state` value/display objects
        self.states = loadAll();
        self.selectedItem = null;
        self.searchText = null;
        self.querySearch = querySearch;

        function querySearch(query) {
            var results = query ? self.states.filter(createFilterFor(query)) : self.states;
            var deferred = $q.defer();
            $timeout(function () { deferred.resolve(results); }, Math.random() * 1000, false);
            return deferred.promise;
        }

        /**
            *TODO: replace with real quering function
            */
        function loadAll() {
            var allStates = 'Alabama, Alaska, Arizona, Arkansas, California, Colorado, Connecticut, Delaware,\
                  Florida, Georgia, Hawaii, Idaho, Illinois, Indiana, Iowa, Kansas, Kentucky, Louisiana,\
                  Maine, Maryland, Massachusetts, Michigan, Minnesota, Mississippi, Missouri, Montana,\
                  Nebraska, Nevada, New Hampshire, New Jersey, New Mexico, New York, North Carolina,\
                  North Dakota, Ohio, Oklahoma, Oregon, Pennsylvania, Rhode Island, South Carolina,\
                  South Dakota, Tennessee, Texas, Utah, Vermont, Virginia, Washington, West Virginia,\
                  Wisconsin, Wyoming';

            return allStates.split(/, +/g).map(function (state) {
                return {
                    value: state.toLowerCase(),
                    display: state
                };
            });
        }

        function createFilterFor(query) {
            var lowercaseQuery = angular.lowercase(query);

            return function filterFn(state) {
                return (state.value.indexOf(lowercaseQuery) === 0);
            };

        }

    });

    app.controller("GalleryController", function ($scope, $rootScope,
        $mdSidenav, $mdDialog, $log, $mdComponentRegistry, booksService) {

        $scope.viewFullscreen = true;

        $rootScope.$on('Refresh', function (event, books) {
            $scope.setTiles(books);
        });

        $scope.tiles = [];

        $scope.refresh = function () {
            booksService.query()
                .then(function (data) {
                    $scope.setTiles(data);
                })
        }

        $scope.setTiles = function (data) {
            var books = data;
            var it, results = [];
            for (var tile = 0; tile < books.length; tile++) {

                it = {};
                it.id = tile + 1;
                it.name = books[tile].Name;
                it.author = books[tile].Author;
                it.publisher = books[tile].Publisher
                it.description = books[tile].Description;
                it.span = { row: 1, col: 1 };
                it.ISBN = books[tile].ISBN
                it.image = books[tile].Photos[0];
                it.photos = books[tile].Photos;
                it.price = books[tile].Price;
                it.illustrator = books[tile].Illustrator;
                it.series = books[tile].Series;
                it.language = books[tile].Language;
                it.size = books[tile].Size;
                it.weight = books[tile].Weight;

                results.push(it);
            }
            $scope.tiles = results;
        }

        $scope.refresh();

        $scope.addToCart = function (ev, tile) {
            $rootScope.$emit('AddBook', [tile]);
        };

        $scope.showAdvanced = function (ev, tile) {
            $mdDialog.show({
                controller: DialogController,
                templateUrl: '../Content/DetailsView.TMPL.html',
                parent: angular.element(document.body),
                targetEvent: ev,
                clickOutsideToClose: true,
                fullscreen: $scope.viewFullscreen, // Only for -xs, -sm breakpoints.
                locals: {
                    tile: tile
                }
            })
                .then(function (answer) {
                    $scope.status = 'You said the information was "' + answer + '".';
                }, function () {
                    $scope.status = 'You cancelled the dialog.';
                });
        };

        function DialogController($scope, $mdDialog, $rootScope, tile) {

            $scope.tile = tile;

            $scope.hide = function () {
                $mdDialog.hide();
            };

            $scope.cancel = function () {
                $mdDialog.cancel();
            };

            $scope.answer = function (answer) {
                $mdDialog.hide(answer);
            };

            $scope.addThisToCart = function (ev, book) {
                $rootScope.$emit('AddBook', [book]);
            }
        }
    });

    app.controller('CartController', function ($scope, $timeout, $rootScope,
        $mdComponentRegistry, $mdSidenav, $log) {

        $scope.books = [];
        $scope.totalAmount = 0;

        $rootScope.$on('AddBook', function (event, book) {
            $scope.addBook(book);
        });

        $scope.addBook = function (book) {
            if ($scope.books.length >= 1) {
                for (var index = 0; index < $scope.books.length; index++) {
                    if ($scope.books[index].id == book[0].id) {
                        return;
                    }
                }
            }

            newBook = book[0];
            newBook.quantity = 1;
            newBook.totalPrice = book[0].price;

            $scope.books.push(newBook);
            $log.debug("CART: new book added");

            $scope.totalAmount += newBook.totalPrice;
            changeAmount($scope.totalAmount);
        };

        $scope.removeBook = function (bookId) {

            for (var index = 0; index < $scope.books.length; index++) {
                if ($scope.books[index].id == bookId) {

                    $scope.totalAmount -= $scope.books[index].totalPrice;
                    changeAmount($scope.totalAmount);

                    $scope.books.splice(index, 1);

                    $log.debug("CART: book removed");
                }
            }
        }

        $scope.changeQuantity = function (bookId, quantity) {
            for (var book = 0; book < $scope.books.length; book++) {
                if ($scope.books[book].id == bookId) {

                    if ($scope.books[book].quantity == 1 && quantity == -1) return;

                    $scope.totalAmount -= $scope.books[book].totalPrice;

                    $scope.books[book].quantity += quantity;
                    $scope.books[book].totalPrice = $scope.books[book].price * $scope.books[book].quantity;

                    $scope.totalAmount += $scope.books[book].totalPrice;
                    changeAmount($scope.totalAmount);

                    $log.debug("CART: quantity changed, new quantity: " + $scope.books[book].quantity);
                }
            }
        };

        $scope.close = function () {
            // Component lookup should always be available since we are not using `ng-if`
            $mdSidenav('right').close()
                .then(function () {
                    $log.debug("close RIGHT is done");
                });
        };

        $rootScope.$on("OpenCart", function () {
            $scope.openCart();
        });

        $scope.openCart = buildToggler('right');

        function buildToggler(navID) {
            return function () {
                // Component lookup should always be available since we are not using `ng-if`
                $mdComponentRegistry.when('left').then(function () {
                    $mdSidenav(navID)
                        .toggle()
                        .then(function () {
                            $log.debug("toggle " + navID + " is done");
                        });
                })

            };
        }

        function changeAmount(amount) {
            document.getElementById("odometer").innerHTML = amount;
        }
    });

    app.controller("FooterController", function ($scope) {
        $scope.contact = {
            name: '',
            email: '',
            message: ''
        };
    });
    // UI Effects 
    app.controller("ScrollController", ['$scope', '$location', '$anchorScroll',
        function ($scope, $location, $anchorScroll) {

            $scope.ScrollToGallery = function () {

                $location.hash('gallery');

                $anchorScroll();
            };

        }]);
    // UI Effects
    app.factory('booksService', function ($http, $q) {
        return {
            query: function () {
                var deferred = $q.defer();

                $http.get('/Home/GetBooks')
                    .success(function (data) {
                        console.log("success:" + data)
                        deferred.resolve(data);
                    })
                    .error(function (data) {
                        console.log("error:" + data)
                        deferred.reject(data);
                    });

                return deferred.promise;
            }
        };
    });

    app.factory('authorFilters', function ($http, $q) {
        return {
            query: function () {
                var deferred = $q.defer();

                $http.get('/Home/GetAuthorFilters')
                    .success(function (data) {
                        console.log("success:" + data)
                        deferred.resolve(data);
                    })
                    .error(function (data) {
                        console.log("error:" + data)
                        deferred.reject(data);
                    });

                return deferred.promise;
            }
        };
    });

    app.factory('publisherFilters', function ($http, $q) {
        return {
            query: function () {
                var deferred = $q.defer();

                $http.get('/Home/GetPublisherFilters')
                    .success(function (data) {
                        console.log("success:" + data)
                        deferred.resolve(data);
                    })
                    .error(function (data) {
                        console.log("error:" + data)
                        deferred.reject(data);
                    });

                return deferred.promise;
            }
        };
    });

    app.factory('categoryFilters', function ($http, $q) {
        return {
            query: function () {
                var deferred = $q.defer();

                $http.get('/Home/GetCategoryFilters')
                    .success(function (data) {
                        console.log("success:" + data)
                        deferred.resolve(data);
                    })
                    .error(function (data) {
                        console.log("error:" + data)
                        deferred.reject(data);
                    });

                return deferred.promise;
            }
        };
    });

    app.factory('filterBooks', function ($http, $q) {
        return {
            query: function (filters) {
                var deferred = $q.defer();

                $http.post('/Home/GetFilteredBooks', filters)
                    .success(function (data) {
                        console.log("success:" + data)
                        deferred.resolve(data);
                    })
                    .error(function (data) {
                        console.log("error:" + data)
                        deferred.reject(data);
                    });

                return deferred.promise;
            }
        };
    });
})();