'use strict';

module.exports = function(grunt) {

    // Load grunt tasks automatically
    require('load-grunt-tasks')(grunt);

    // Time how long tasks take. Can help when optimizing build times
    require('time-grunt')(grunt);

    // Define the configuration for all the tasks
    grunt.initConfig({
        paths: {
            app: 'Scripts/app',
            vendor: 'Scripts/vendor',
            vendorOthers: 'Scripts/vendor-others', // TODO we have to get rid of this folder later on
            scripts: {
                app: [
                    '<%= paths.app %>/config.js',
                    '<%= paths.app %>/app.js',
                    '<%= paths.app %>/app.config.js',
                    '<%= paths.app %>/app.lazyload.js',
                    '<%= paths.app %>/app.router.js',
                    '<%= paths.app %>/app.run.js',
                    '<%= paths.app %>/services/services.js',
                    '<%= paths.app %>/filters/filters.js',
                    '<%= paths.app %>/directives/directives.js',
                    '<%= paths.app %>/controllers/Admin/Admin.js',
                    '<%= paths.app %>/controllers/CallAdmin/callAdmin.js',
                    '<%= paths.app %>/controllers/DashBoard/DashBoard.js',
                    '<%= paths.app %>/controllers/Home/Home.js',
                    '<%= paths.app %>/controllers/Quote/quote.js',
                    '<%= paths.app %>/controllers/Shared/Shared.js',
                    '<%= paths.app %>/controllers/TeleSale/Telesale.js',
                    '<%= paths.app %>/controllers/Lead/Lead.js',
                    '<%= paths.app %>/**/*.js',
                    '!<%= paths.app %>/**/*.test.js',
                    '!<%= paths.app %>/test/{,*/}*'
                ],
                vendor: [
                    '<%= paths.vendor %>/jquery/dist/jquery.js',
                    '<%= paths.vendor %>/toastr/toastr.js',
                    '<%= paths.vendor %>/twitter-bootstrap-wizard/jquery.bootstrap.wizard.js',
                    '<%= paths.vendor %>/bootstrap/dist/js/bootstrap.js',
                    '<%= paths.vendor %>/sweetalert/dist/sweetalert-dev.js',
                    '<%= paths.vendor %>/screenfull/dist/screenfull.js',
                    '<%= paths.vendor %>/moment/moment.js',
                    '<%= paths.vendor %>/bootstrap-daterangepicker/daterangepicker.js',
                    '<%= paths.vendor %>/footable/dist/footable.all.min.js',
                    '<%= paths.vendor %>/lodash/lodash.js',
                    '<%= paths.vendor %>/angular/angular.js',
                    '<%= paths.vendor %>/angular-animate/angular-animate.js',
                    '<%= paths.vendor %>/angular-cookies/angular-cookies.js',
                    '<%= paths.vendor %>/angular-resource/angular-resource.js',
                    '<%= paths.vendor %>/angular-sanitize/angular-sanitize.js',
                    '<%= paths.vendor %>/angular-ui-router/release/angular-ui-router.js',
                    '<%= paths.vendor %>/angular-filter/dist/angular-filter.js',
                    '<%= paths.vendor %>/angular-ui-utils/ui-utils.js',
                    '<%= paths.vendor %>/angular-bootstrap/ui-bootstrap-tpls.js',
                    '<%= paths.vendor %>/angular-loading-bar/build/loading-bar.js',
                    '<%= paths.vendor %>/ngstorage/ngStorage.js',
                    '<%= paths.vendor %>/angular-http-auth/src/http-auth-interceptor.js',
                    '<%= paths.vendor %>/angular-ui-select/dist/select.js',
                    '<%= paths.vendor %>/angular-xeditable/dist/js/xeditable.js',
                    '<%= paths.vendor %>/ng-table/dist/ng-table.js',
                    '<%= paths.vendor %>/ng-file-upload/ng-file-upload-all.js',
                    '<%= paths.vendorOthers %>/**.js'
                ]
            }
        },

        // Make sure code styles are up to par and there are no obvious mistakes
        jshint: {
            options: {
                jshintrc: '.jshintrc',
                reporter: require('jshint-stylish')
            },
            all: {
                src: [
                    'Gruntfile.js',
                    '<%= paths.app %>/controllers/Quote/**/*.js',
                    '!<%= paths.app %>/controllers/Quote/Cost/**/*.js',
                    '!<%= paths.app %>/controllers/Quote/Cost_v2/**/*.js'
                ]
            }
        },

        // Test settings
        karma: {
            options: {
                files: [
                    '<%= paths.scripts.vendor %>',
                    '<%= paths.vendor %>/angular-mocks/angular-mocks.js',
                    '<%= paths.scripts.app %>',
                    '<%= paths.app %>/**/*.test.js',
                    '<%= paths.app %>/test/{,*/}*'
                ],
                frameworks: ['jasmine'],
                port: 9186,
                plugins: [
                    'karma-chrome-launcher',
                    'karma-phantomjs-launcher',
                    'karma-jasmine',
                    'karma-coverage',
                    'karma-html-reporter'
                ],
                colors: true,
                browsers: ['PhantomJS'],
                singleRun: true
            },
            local: {
                reporters: ['coverage', 'dots', 'html'],
                preprocessors: {
                    '<%= paths.app %>/**/!(*test).js': ['coverage']
                },
                coverageReporter: {
                    dir: 'reports',
                    subdir: 'coverage',
                    reporters: [
                        { type: 'text-summary' },
                        { type: 'html' }
                    ]
                }
            },
            debug: {
                singleRun: false,
                autoWatch: true,
                reporters: ['dots'],
                browsers: ['Chrome']
            }
        }
    });

    // Runs unit tests in a headless browser only once, no debugging supported and simplified code coverage
    grunt.registerTask('test', [
        'karma:local'
    ]);

    // Runs unit tests in Chrome in a loop with debugging support
    grunt.registerTask('test:debug', [
        'karma:debug'
    ]);

  
};