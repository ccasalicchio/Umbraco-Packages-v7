angular.module("umbraco")
.controller("Ad.Preview",
	function ($scope, contentEditingHelper, editorState) {
		'use strict';
		var engine = {
			url:'',
			txt:'',
			layout:'',
			extReferrer:'',
			txtLocation:'',
			tooltip:'',
			caption:'',
			class:''
		};

		var content = editorState.current;
		var properties = contentEditingHelper.getAllProps(content);
		engine.url = _.findWhere(properties, { alias: "umbracoFile" }).value;
		engine.txt = _.findWhere(properties, { alias: "umbracoFile_AdText" }).value;
		engine.layout = _.findWhere(properties, { alias: "umbracoFile_AdDisplayText" }).value;
		engine.extReferrer = _.findWhere(properties, { alias: "umbracoFile_adExtRef" }).value;

		if(engine.extReferrer==='') engine.extReferrer="#";

		switch(engine.layout){
			case '934':
			break;
			case '935':
			engine.caption=engine.txt;
			engine.class='overlay';
			break;
			case '936':
			engine.tooltip=engine.txt;
			engine.class='tooltip';
			break;
			case '937':
			engine.caption=engine.txt;
			engine.class='caption';
			break;
		}

		$scope.engine = engine;

	});