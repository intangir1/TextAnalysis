package com.likhtman.TextAnalysis.Callbacks;

public interface CallbacksWebApi {
    void onAboutToBegin();
    void onSuccess(String downloadedText, int httpStatusCode);
    void onError(int httpStatusCode, String errorMessage);
}
