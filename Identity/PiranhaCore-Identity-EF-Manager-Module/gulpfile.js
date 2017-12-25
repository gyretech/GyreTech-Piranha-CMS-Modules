"use strict";

var gulp = require("gulp"),
    _ = require("lodash"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    htmlmin = require("gulp-htmlmin"),
    uglify = require("gulp-uglify"),
    merge = require("merge-stream"),
    rimraf = require("gulp-rimraf"),
    del = require("del"),
    ignore = require('gulp-ignore'),
    bundleconfig = require("./bundleconfig.json");

var paths = {
    libs: "./node_modules/",
    webroot: "./assets/lib/"
};

var deps = {
    "knockout": {
        "build/output/*": ""
    },
    "knockout-mapping": {
        "dist/*": ""
    },
    "font-awesome": {
        "css/*": "css",
        "fonts/*": "fonts"
    }
};


var regex = {
    css: /\.css$/,
    html: /\.(html|htm)$/,
    js: /\.js$/
};

gulp.task("min", ["min:js", "min:css", "min:html"]);

gulp.task("min:js", function () {
    var tasks = getBundles(regex.js).map(function (bundle) {
        return gulp.src(bundle.inputFiles, { base: "." })
            .pipe(concat(bundle.outputFileName))
            .pipe(uglify())
            .pipe(gulp.dest("."));
    });
    return merge(tasks);
});

gulp.task("min:css", function () {
    var tasks = getBundles(regex.css).map(function (bundle) {
        return gulp.src(bundle.inputFiles, { base: "." })
            .pipe(concat(bundle.outputFileName))
            .pipe(cssmin())
            .pipe(gulp.dest("."));
    });
    return merge(tasks);
});

gulp.task("min:html", function () {
    var tasks = getBundles(regex.html).map(function (bundle) {
        return gulp.src(bundle.inputFiles, { base: "." })
            .pipe(concat(bundle.outputFileName))
            .pipe(htmlmin({ collapseWhitespace: true, minifyCSS: true, minifyJS: true }))
            .pipe(gulp.dest("."));
    });
    return merge(tasks);
});

gulp.task("clean", function () {
    var files = bundleconfig.map(function (bundle) {
        return bundle.outputFileName;
    });

    return del(files);
});

gulp.task("watch", function () {
    getBundles(regex.js).forEach(function (bundle) {
        gulp.watch(bundle.inputFiles, ["min:js"]);
    });

    getBundles(regex.css).forEach(function (bundle) {
        gulp.watch(bundle.inputFiles, ["min:css"]);
    });

    getBundles(regex.html).forEach(function (bundle) {
        gulp.watch(bundle.inputFiles, ["min:html"]);
    });
});

function getBundles(regexPattern) {
    return bundleconfig.filter(function (bundle) {
        return regexPattern.test(bundle.outputFileName);
    });
}

gulp.task("cleanlibs",
    function(cb) {
        return gulp.src(paths.webroot  + '*', { read: false })
            .pipe(ignore(paths.webroot + '/site'))
            .pipe(rimraf());
        
    });

gulp.task("scripts", ["cleanlibs"], function () {

    _.forIn(deps,
        (val, prop) => {
            console.log("Prepping Scripts for: " + prop);

            _.forIn(deps[prop], (x, itemProp) => {
                gulp.src(paths.libs + prop + "/" + itemProp)
                    .pipe(gulp.dest(paths.webroot + prop + "/" + deps[prop][itemProp]));
            });
        });

});