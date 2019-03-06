var gulp = require("gulp"),
	fs = require("fs"),
	sass = require("gulp-sass");

// other content removed

gulp.task("sass",
	function() {
		return gulp.src("Styles/finplan.scss")
			.pipe(sass())
			.pipe(gulp.dest("wwwroot/css"));
	});
gulp.task("sass-form-signin",
	function() {
		return gulp.src("Styles/form-signin.scss")
			.pipe(sass())
			.pipe(gulp.dest("wwwroot/css"));
	});

gulp.task("watch",
	function() {
		gulp.watch("Styles/**/*.scss", gulp.parallel("sass","sass-form-signin"));
		// Other watchers
	}); 