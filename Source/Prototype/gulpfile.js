/// <binding BeforeBuild="default" Clean="clean" ProjectOpened="watch" />
"use strict";
{
	const del = require("del");
	const fileSystem = require("fs");
	const gulp = require("gulp");
	const path = require("path");
	const plumber = require("gulp-plumber");
	const rename = require("gulp-rename");
	const rollup = require("rollup");
	const rollupCommonJs = require("rollup-plugin-commonjs");
	const rollupNodeResolver = require("rollup-plugin-node-resolve");
	const rollupTypescript = require("rollup-plugin-typescript");
	const rollupUglify = require("rollup-plugin-uglify");
	const sass = require("gulp-sass");
	const svgSrite = require("gulp-svg-sprite");
	const sourcemaps = require("gulp-sourcemaps");

	sass.compiler = require("node-sass");

	const applicationDestinationRootDirectory = "../Application";
	const destinationRootDirectoryName = "wwwroot";
	const fontsDirectoryName = "Fonts";
	const iconsDirectoryName = "Icons";
	const imagesDirectoryName = "Images";
	const librariesDirectoryName = "Libraries";
	const scriptsDirectoryName = "Scripts";
	const styleDirectoryName = "Style";

	const applicationScriptsDestinationDirectory = path.join(applicationDestinationRootDirectory, scriptsDirectoryName);
	const applicationSpritesDestinationDirectory = path.join(applicationDestinationRootDirectory, styleDirectoryName, iconsDirectoryName);
	const applicationStyleDestinationDirectory = path.join(applicationDestinationRootDirectory, styleDirectoryName);
	const fontsDestinationDirectory = path.join(destinationRootDirectoryName, styleDirectoryName, fontsDirectoryName);
	const fontsSourceDirectory = path.join(styleDirectoryName, fontsDirectoryName);
	const iconsSourceDirectory = path.join(styleDirectoryName, iconsDirectoryName);
	const imagesDestinationDirectory = path.join(destinationRootDirectoryName, styleDirectoryName, imagesDirectoryName);
	const imagesSourceDirectory = path.join(styleDirectoryName, imagesDirectoryName);
	const scriptLibrariesDestinationDirectory = path.join(destinationRootDirectoryName, scriptsDirectoryName, librariesDirectoryName);
	const scriptLibrariesSourceDirectory = path.join(scriptsDirectoryName, librariesDirectoryName);
	const scriptsDestinationDirectory = path.join(destinationRootDirectoryName, scriptsDirectoryName);
	const scriptsSourceDirectory = scriptsDirectoryName;
	const spriteDestinationDirectory = path.join(destinationRootDirectoryName, styleDirectoryName, iconsDirectoryName);
	const styleDestinationDirectory = path.join(destinationRootDirectoryName, styleDirectoryName);
	const styleSourceDirectory = styleDirectoryName;

	async function buildScriptBundle() {
		console.log("Building script-bundle...");

		deleteIfExists(scriptsDestinationDirectory);

		var bundleName = "Site.js";

		await rollup.rollup(createRollupInputOptions()).then(async bundle => {
			await writeRollupBundle(bundle, path.join(scriptsDestinationDirectory, bundleName), true);
			await writeRollupBundle(bundle, path.join(applicationScriptsDestinationDirectory, bundleName), true);
		});

		await rollup.rollup(createRollupInputOptions(true)).then(async bundle => {
			bundleName = bundleName.replace(".", ".min.");

			await writeRollupBundle(bundle, path.join(scriptsDestinationDirectory, bundleName));
			await writeRollupBundle(bundle, path.join(applicationScriptsDestinationDirectory, bundleName));
		});
	}

	function buildSprite() {
		console.log("Building sprite...");

		deleteIfExists(spriteDestinationDirectory);

		const spriteFileName = "sprite.svg";
		
		return gulp.src(replaceBackSlashWithForwardSlash(path.join(iconsSourceDirectory, "**/*.svg")))
			.pipe(plumber())
			.pipe(svgSrite({
				mode: {
					symbol: {
						dest: "",
						render: false,
						sprite: spriteFileName
					}
				}
			}))
			.on("error",
				function(error) {
					if (!error)
						return;

					const errorMessage = error.message || error;

					log.error("Failed to compile sprite.", errorMessage.toString());
					this.emit("end");
				})
			.pipe(gulp.dest(spriteDestinationDirectory))
			.pipe(gulp.dest(applicationSpritesDestinationDirectory));
	}

	function buildStyleSheets() {
		console.log("Building style-sheets...");

		del(replaceBackSlashWithForwardSlash(path.join(styleDestinationDirectory, "**/*.css")));

		return gulp.src(replaceBackSlashWithForwardSlash(path.join(styleSourceDirectory, "Site.scss")))
			.pipe(sourcemaps.init())
			.pipe(sass(createSassOptions()).on("error", sass.logError))
			.pipe(sourcemaps.write())
			.pipe(gulp.dest(styleDestinationDirectory))
			.pipe(gulp.dest(applicationStyleDestinationDirectory))
			.pipe(sass(createSassOptions(true)).on("error", sass.logError))
			.pipe(rename({ suffix: ".min" }))
			.pipe(gulp.dest(styleDestinationDirectory))
			.pipe(gulp.dest(applicationStyleDestinationDirectory));
	}

	function clean(done) {
		console.log(`Deleting directory \"${scriptsDestinationDirectory}\"...`);
		console.log(`Deleting directory \"${styleDestinationDirectory}\"...`);

		del.sync([scriptsDestinationDirectory, styleDestinationDirectory]);

		done();
	};

	function copyFonts() {
		console.log("Copying fonts...");

		deleteIfExists(fontsDestinationDirectory);

		return gulp.src(path.join(fontsSourceDirectory, "**/*"))
			.pipe(gulp.dest(destinationRootDirectoryName))
			.pipe(gulp.dest(applicationDestinationRootDirectory));
	}

	function copyImages() {
		console.log("Copying images...");

		deleteIfExists(imagesDestinationDirectory);

		return gulp.src(path.join(imagesSourceDirectory, "**/*"))
			.pipe(gulp.dest(destinationRootDirectoryName))
			.pipe(gulp.dest(applicationDestinationRootDirectory));
	}

	function copyScriptLibraries() {
		console.log("Copying script-libraries...");

		deleteIfExists(scriptLibrariesDestinationDirectory);

		return gulp.src(path.join(scriptLibrariesSourceDirectory, "**/*.js"))
			.pipe(gulp.dest(destinationRootDirectoryName))
			.pipe(gulp.dest(applicationDestinationRootDirectory));
	}

	function createRollupInputOptions(minify) {
		const plugins = [
			rollupNodeResolver(),
			rollupCommonJs(),
			rollupTypescript()
		];

		if (minify) {
			plugins.push(rollupUglify.uglify());
		}

		return {
			input: path.join(scriptsSourceDirectory, "Site.ts"),
			plugins: plugins
		};
	}

	function createRollupOutputOptions(file, sourcemap) {
		const format = "iife";
		const name = "oncology.web";

		return {
			file: file,
			format: format,
			name: name,
			sourcemap: sourcemap ? "inline" : false
		};
	}

	function createSassOptions(compressed) {
		return {
			includePaths: ["node_modules"],
			indentType: "tab",
			indentWidth: 1,
			linefeed: "crlf",
			outputStyle: compressed ? "compressed" : "expanded"
		};
	}

	function deleteIfExists(pathToDelete)
	{
		if (fileSystem.existsSync(pathToDelete))
			del.sync(pathToDelete);
	}

	function replaceBackSlashWithForwardSlash(value) {
		return value.replace(/\\/g, "/");
	}

	function watchFonts() {

		const patterns = [
			replaceBackSlashWithForwardSlash(path.join(fontsSourceDirectory, "**/*"))
		];

		gulp.watch(patterns, copyFonts);
	}

	function watchIcons() {

		const patterns = [
			replaceBackSlashWithForwardSlash(path.join(iconsSourceDirectory, "**/*.svg"))
		];

		gulp.watch(patterns, buildSprite);
	}

	function watchImages() {

		const patterns = [
			replaceBackSlashWithForwardSlash(path.join(imagesSourceDirectory, "**/*"))
		];

		gulp.watch(patterns, copyImages);
	}

	function watchSass() {

		const patterns = [
			replaceBackSlashWithForwardSlash(path.join(styleSourceDirectory, "**/*.scss"))
		];

		gulp.watch(patterns, buildStyleSheets);
	}

	function watchScriptLibraries() {

		const patterns = [
			replaceBackSlashWithForwardSlash(path.join(scriptLibrariesSourceDirectory, "**/*.js"))
		];

		gulp.watch(patterns, copyScriptLibraries);
	}

	function watchScripts() {

		const patterns = [
			replaceBackSlashWithForwardSlash(path.join(scriptsSourceDirectory, "**/*.js")),
			replaceBackSlashWithForwardSlash(path.join(scriptsSourceDirectory, "**/*.ts"))
		]; 
		
		gulp.watch(patterns, buildScriptBundle);
	}

	async function writeRollupBundle(bundle, file, sourcemap) {
		return bundle.write(createRollupOutputOptions(file, sourcemap)).then(console.log(" - writing \"" + file + "\"..."));
	}

	gulp.task("build-script-bundle", buildScriptBundle);

	gulp.task("build-sprite", gulp.series(buildSprite, watchIcons));

	gulp.task("build-style-sheets", buildStyleSheets);

	gulp.task("clean", gulp.series(clean));
	
	gulp.task("copy-fonts", gulp.series(copyFonts, watchFonts));

	gulp.task("copy-images", gulp.series(copyImages, watchImages));

	gulp.task("copy-script-libraries", gulp.series(copyScriptLibraries, watchScriptLibraries));

	gulp.task("default", gulp.parallel(buildScriptBundle, buildSprite, buildStyleSheets, copyFonts, copyImages, copyScriptLibraries));

	gulp.task("watch", gulp.parallel(watchFonts, watchIcons, watchImages, watchSass, watchScriptLibraries, watchScripts));
}