mergeInto(LibraryManager.library, {
    GetURLParameter: function(paramNamePtr) {
        var paramName = UTF8ToString(paramNamePtr); // Convert pointer to string
        var urlParams = new URLSearchParams(window.location.search);
        var paramValue = urlParams.get(paramName);
        
        if (paramValue === null) return 0; // Return 0 (null pointer) if not found

        // Allocate memory on heap for the result string
        var lengthBytes = lengthBytesUTF8(paramValue) + 1;
        var stringOnHeap = _malloc(lengthBytes);
        stringToUTF8(paramValue, stringOnHeap, lengthBytes); // Copy string to heap
        return stringOnHeap; // Return pointer to heap memory
    },
    FreeMemory: function(ptr) {
        _free(ptr); // Free allocated memory
    }
});