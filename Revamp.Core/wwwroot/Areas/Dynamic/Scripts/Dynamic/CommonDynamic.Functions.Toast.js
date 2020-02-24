///this file should be used for all toast notification strings to keep things consistent.  strongly consider using this even for one-off notifications to keep strings out of your js
var ToastConstants = {

    //used to notify user that the attempted delete succeeded
    docCloneSuccess: { type: 'success', title: 'SUCCESS', msg: 'Document was successfully cloned.' },

    //used to notify user that the attempted delete failed
    docCloneFail: { type: 'success', title: 'ERROR', msg: 'An error ocurred when trying to clone this document.' },

    //used to notify user that the attempted delete succeeded
    docDeleteSuccess: { type: 'success', title: 'SUCCESS', msg: 'Document was successfully deleted.' },

    //used to notify user that the attempted delete failed
    docDeleteFail: { type: 'success', title: 'ERROR', msg: 'An error ocurred when trying to delete this document.' },

    //used to notify user that the attempted delete failed because they don't have privileges
    deletionNotPermitted: { type: 'error', title: 'ERROR', msg: 'You do not have the necessary permissions to delete this record.' },

    //used to notify user that the attempted delete failed
    deletionError: { type: 'error', title: 'ERROR', msg: 'An error ocurred when trying to delete this record.' },

    //used to notify user that the attempted delete succeeded
    deletionSuccess: { type: 'success', title: 'SUCCESS', msg: 'Record successfully deleted.' },

    //used mostly when you're supplying a custom detailed error message, just here to standardize headings and class
    genericError: { type: 'error', title: 'ERROR', msg: 'An error has occurred.' },

    //used mostly when you're supplying a custom detailed success message, just here to standardize headings and class
    genericSuccess: { type: 'success', title: 'SUCCESS', msg: 'Operation completed successfully.' },

    //user tried to delete a document but it can't be deleted because children records exist
    documentCannotBeDeletedWithChildren: { type: 'error', title: 'ERROR', msg: 'You cannot delete this document until you remove all DELETE positions.' },

    //used to notify user that user uic assignment they saved has succeeded
    userUicsSaved: { type: 'success', title: 'SUCCESS', msg: 'User UICs have been saved.' },

    //user tried to submit a form with validation errors
    validationFailed: { type: 'error', title: 'ERROR', msg: 'You cannot save until you correct all validation errors.' },

    //used to notify user paragraph reordering was successful
    paraReorderSuccess: { type: 'success', title: 'SUCCESS', msg: 'Paragraph Reorder successfully saved.' },

    //used to notify user paragraph reordering was a failure
    paraReorderFail: { type: 'error', title: 'ERROR', msg: 'An error occurred when attempting to save the reordered paragraphs.' },

    //used to notify user paragraph edit was a failure
    paraEditFail: { type: 'error', title: 'ERROR', msg: 'An error occurred when attempting to save the paragraph.' },

    //used to notify user detail save was a failure
    detailSaveFail: { type: 'error', title: 'ERROR', msg: 'An error occurred when attempting to save detail(s).' },

    //used to notify user paragraph reversion was successful
    paraRevertSuccess: { type: 'success', title: 'SUCCESS', msg: 'Paragraph has been reverted to BASE.' },

    //used to notify user paragraph reversion was a failure
    paraRevertFail: { type: 'error', title: 'ERROR', msg: 'An error occurred while attempting to revert the selected paragraph.' },

    //used to notify user paragraph delete was a failure
    paraDeleteFail: { type: 'error', title: 'ERROR', msg: 'An error occurred while attempting to delete the selected paragraph.' },

    //used to notify user paragraph addition was successful
    paraAddSuccess: { type: 'success', title: 'SUCCESS', msg: 'New paragraph successfully added.' },

    //used to notify user paragraph edit was successful
    paraEditSuccess: { type: 'success', title: 'SUCCESS', msg: 'Paragraph edit saved.' },

    //used to notify user paragraph delete was successful
    paraDeleteSuccess: { type: 'success', title: 'SUCCESS', msg: 'Paragraph deleted.' },


    //used to notify user that an "ADD" detail was reverted (so really it was deleted)
    addedDetailReverted: { type: 'success', title: 'SUCCESS', msg: 'This detail was deleted because it was in an "ADD" state when reverted.' },

    //used to notify user that a detail was reverted to base
    detailReverted: { type: 'success', title: 'SUCCESS', msg: 'Detail successfully reverted to base.' },

    //used to notify user that a detail was saved successfully
    detailSaveSuccess: { type: 'success', title: 'SUCCESS', msg: 'Detail(s) successfully saved.' },

    //used to notify user that a detail was deleted successfully
    detailDeleted: { type: 'success', title: 'SUCCESS', msg: 'Detail successfully deleted.' },

    //used to notify user that a detail was successfully moved to another document
    detailMoveSuccess: { type: 'success', title: 'SUCCESS', msg: 'Detail was successfully moved to another document.' },

    //used to notify user that a detail was successfully moved to another document
    detailMoveFail: { type: 'error', title: 'ERROR', msg: 'An error occurred while attempting to move this detail to another document.' },

    //used to notify user account unlock was successful
    accountUnlockSuccess: { type: 'success', title: 'SUCCESS', msg: 'The user\'s account has been unlocked.' },

    //used to notify user account unlock was not successful
    accountUnlockError: { type: 'error', title: 'ERROR', msg: 'There was an error when attempting to unlock this user\'s account.' },

    //document status was changed
    docStatusUpdated: { type: 'success', title: 'SUCCESS', msg: 'Document status successfully updated.' },

    //used to notify user account unlink was successful
    accountUnlinkSuccess: { type: 'success', title: 'SUCCESS', msg: 'The user\'s account has been unlinked.' },

    //used to notify user account unlink was not successful
    accountUnlinkError: { type: 'error', title: 'ERROR', msg: 'There was an error when attempting to unlink this user\'s account.' },

    //used to notify user windows account link was successful
    accountLinkSuccess: { type: 'success', title: 'SUCCESS', msg: 'The user\'s windows account has been linked.' },

    //used to notify cac/edipi account link was successful
    cacLinkSuccess: { type: 'success', title: 'SUCCESS', msg: 'The user\'s CAC/EDIPI has been linked.' },

    //used to notify permissions clone was successful
    copyUserRightsSuccess: { type: 'success', title: 'SUCCESS', msg: 'The user\'s rights were successfully copied.' },

    //used when the action filter returns a 403 (forbidden)
    noPrivilege: { type: 'error', title: 'ERROR', msg: 'You do not have the required privilege to perform the requested operation.' },

    passwordResetSuccess: { type: 'success', title: 'SUCCESS', msg: 'Your password has been successfully reset.' },

    //used to notify user account disable was successful
    accountDisableSuccess: { type: 'success', title: 'SUCCESS', msg: 'The user\'s account has been disabled.' },

    //used to notify user account disable was not successful
    accountDisableError: { type: 'error', title: 'ERROR', msg: 'There was an error when attempting to disable this user\'s account.' },

    personVacancyCreated: { type: 'success', title: 'SUCCESS', msg: 'A new vacancy was created based on the selected person\'s record.' }
}