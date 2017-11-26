/*
 * Copyright (c) 2017 Keshwar White
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 */

var gulp = require("gulp"),
    less = require("gulp-less"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    rename = require("gulp-rename"),
    uglify = require("gulp-uglify");

var paths = {
    koJS: [
        "assets/lib/knockout/dist/knockout.js",
        "assets/lib/knockout-mapping/knockout.mapping.js"
    ],
    koJSDest: "assets/js/koscript.js"
}

gulp.task("min:kojs",
    function() {
        return gulp.src(paths.koJS, { base: "." })
            .pipe(concat(paths.koJSDest))
            .pipe(gulp.dest("."))
            .pipe(uglify())
            .pipe(rename({
                suffix: ".min"
            }))
            .pipe(gulp.dest("."));
    });

gulp.task("serve", ["min:kojs"]);
gulp.task('default', ["serve"]);