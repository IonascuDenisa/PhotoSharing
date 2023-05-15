// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


// Add event listener for file input change upload images form
document.querySelector('input[name="imageFiles"]').addEventListener('change', (event) => {
    const files = event.target.files;
    let metadataFields = '';

    // Iterate through the selected files
    for (let i = 0; i < files.length; i++) {
        // Read the file and display it as an image
        const reader = new FileReader();
        const filePromise = new Promise((resolve, reject) => {
            reader.onload = function (e) {
                metadataFields += '<li><img src="' + e.target.result + '" alt="' + files[i].name + '" width="200" /></li>';
                resolve();
            }
            reader.readAsDataURL(files[i]);
        });

        filePromise.then(() => {
            // Create input elements for title and description for each file
            metadataFields += '<div class="form-group">';
            metadataFields += '<label for="title-' + i + '">Category</label>';
            metadataFields += '<select name="metadata[' + i + '].Category" class="form-control">';
            metadataFields += '<option value="landscape">Landscape</option>';
            metadataFields += '<option value="nature">Nature</option>';
            metadataFields += '<option value="animals">Animals</option>';
            metadataFields += '<option value="food&drinks">Food & Drinks</option>';
            metadataFields += '<option value="arhitecture">Arhitecture</option>';
            metadataFields += '</select>';
            metadataFields += '</div>';
            metadataFields += '<div class="form-group">';
            metadataFields += '<label for="description-' + i + '">Description</label>';
            metadataFields += '<input type="text" name="metadata[' + i + '].Description" class="form-control" required  />';
            metadataFields += '</div>';

            // Update the metadata fields
            document.getElementById('image-metadata').innerHTML = '<ul>' + metadataFields + '</ul>';
        });
    }
});
