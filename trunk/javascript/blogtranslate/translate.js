﻿// JScript File
function TranslationWidget()
{
	var translated_data = [];
	var Node = null;
	var TimerId = null;
	function positional_translate(position) {
		var v = position;
		return (function(result) {
			if (result.translation) {
				translated_data[v] = result.translation;
			}
			if (result.error) {
				translated_data[v] = result.error.message;
			}
			if (!TimerId) {
				TimerId = setInterval(AssembleData, 1500);
			}
		});
	}
	function AssembleData() {
		Node.html('');
		var nullcount = 0;
		for (var i = 0; i < translated_data.length; i++) {
			if (translated_data[i]) {
				Node.append(translated_data[i]);
			} else {++nullcount;
			}
		}
		if (!nullcount) {
			clearInterval(TimerId);
			translated_data = [];
			TimerId = null;
		}
	}
	function translate_text(text, lan, i) {
		if (text.length > 400) {
			var truckatedString = text.substr(0, 400);
			var last_space = truckatedString.lastIndexOf('.');
			if (last_space < 5) {
				last_space = truckatedString.lastIndexOf('.');
			}
			if (last_space < 5) {
				last_space = 400;
			}
			if (!last_space) {
				last_space = 400;
			}
			translate_text(text.substr(0, last_space), lan, i++);
			translate_text(text.substr(last_space), lan, i);
		} else {
			google.language.translate(text, 'en', lan, positional_translate(i));
		}
	}
	function TranslateHandle() {
		return (function() {
			var text = $(this).parent().next().closest('.post-body').text();
			if (text) {
				/*$('#TranslatedText').html("");*/
				Node = $(this).parent().prev();
				Node.html('<font color="red">Translating...</font>');
				if (TimerId) {
					clearInterval(TimerId);
				}
				translated_data = [];
				translate_text(text, this.value, 0);
			}
		});
	}
	function getTranslatedTextDiv() {
		var ele = document.createElement('div');
		$(ele).attr('class', 'TranslatedText');
		return ele;
	}
	function getTranslationOptionDiv() {
		var ele = document.createElement('div');
		$(ele).html('Translate<select class="translate_option"><option value="en">English</option><option value="hi">Hindi</option><option value="el">Greek</option><option value="sq">Albanian</option><option value="ar">Arabic</option><option value="bg">Bulgarian</option><option value="ca">Catalan</option><option value="zh-CN">Chinese</option><option value="hr">Croatian</option><option value="cs">Czech</option><option value="da">Danish</option><option value="nl">Dutch</option><option value="en">English</option><option value="et">Estonian</option><option value="tl">Filipino</option><option value="fi">Finnish</option><option value="fr">French</option><option value="gl">Galician</option><option value="de">German</option><option value="el">Greek</option><option value="iw">Hebrew</option><option value="hi">Hindi</option><option value="hu">Hungarian</option><option value="id">Indonesian</option><option value="it">Italian</option><option value="ja">Japanese</option><option value="ko">Korean</option></select>');
		$(ele).attr('class', 'translate');
		return ele
	}
	$(document).ready(function() {
		$('.post-body').before(getTranslatedTextDiv());
		$('.post-body').before(getTranslationOptionDiv());
		$('.translate_option').change(TranslateHandle());
	});
}
TranslationWidget();
