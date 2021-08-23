/**
 * @license Copyright (c) 2003-2021, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
	// config.uiColor = '#AADC6E';

    config.syntaxhighlight_lang = 'csharp';
    config.syntaxhighlight_hideControls = true;
    config.languages = 'vi';
    config.filebrowserBrowseUrl = '/Content/admin/assets/js/plugins/ckfinder/ckfinder.html';
    config.filebrowserImageBrowseUrl = '/Content/admin/assets/js/plugins/ckfinder/ckfinder.html?Types=Images';
    config.filebrowserFlashBrowseUrl = '/Content/admin/assets/js/plugins/ckfinder/ckfinder.html?Types=Flash';
    config.filebrowserUploadUrl = '/Content/admin/assets/js/plugins/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=File';
    config.filebrowserImageUploadUrl = '/Uploads';
    config.filebrowserFlashUploadUrl = '/Content/admin/assets/js/plugins/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash';

    CKFinder.setupCKEditor(null, '/Content/admin/assets/js/plugins/ckfinder/');
};
