function showReplyForm( formId, containerId, commentId ){
		var form = document.getElementById(formId);
		
		var hfParentId = document.getElementById(form.HiddenFieldParenId)
		if (hfParentId != null) {
		    if (commentId != null) hfParentId.value = commentId;
		    else hfParentId.value = '';
		}
				
		var container = document.getElementById(containerId);
		container.appendChild( form );
		form.style.display='block';
}