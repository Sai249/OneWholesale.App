$(document).ready(function () {
    console.log("📢 site.js Loaded Successfully!");

    // 🟢 Save or Update Brand
    $("#saveBtn").click(function (e) {
        e.preventDefault();
        console.log("📝 Save Button Clicked");

        const formData = new FormData();
        formData.append("BrandName", $("#BrandName").val());
        formData.append("CompanyLogo", $("#CompanyLogo")[0].files[0]);
        formData.append("__RequestVerificationToken", $('input[name="__RequestVerificationToken"]').val());

        $.ajax({
            url: "/Brand/Create",
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.success) {
                    console.log("✅ Success:", response.message);
                    Toastify({
                        text: response.message,
                        duration: 3000,
                        style: { background: "green" }
                    }).showToast();
                } else {
                    console.log("❌ Error:", response.message);
                    Toastify({
                        text: response.message,
                        duration: 3000,
                        style: { background: "red" }
                    }).showToast();
                }
            },
            error: function (xhr) {
                console.log("❌ AJAX Error:", xhr.status, xhr.responseText);
                Toastify({
                    text: "Error occurred while saving.",
                    duration: 3000,
                    style: { background: "red" }
                }).showToast();
            }
        });
    });

    // ✏️ Update Brand (if dynamic updates are needed)
    $(document).on("click", ".updateButton", function (e) {
        e.preventDefault();
        const brandId = $(this).data("id");
        console.log("🔄 Update Clicked - Brand ID:", brandId);

        $.ajax({
            url: "/Brand/UpdateBrand",
            type: "POST",
            data: { id: brandId },
            headers: {
                "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val()
            },
            success: function (response) {
                console.log("✅ Update Response:", response);
                Toastify({
                    text: response.message || "Updated successfully!",
                    duration: 3000,
                    style: { background: response.success ? "green" : "red" }
                }).showToast();
                if (response.success) location.reload();
            },
            error: function (xhr) {
                console.error("❌ Update Error:", xhr.responseText);
                Toastify({
                    text: "An error occurred while updating.",
                    duration: 3000,
                    style: { background: "red" }
                }).showToast();
            }
        });
    });
});

// ❌ Delete Brand - Globally accessible
function deleteBrand(id) {
    const token = $('input[name="__RequestVerificationToken"]').val();
    if (!confirm("Are you sure you want to delete this brand?")) return;

    fetch(`/Brand/DeleteBrand/${id}`, {
        method: "POST",
        headers: {
            "RequestVerificationToken": token
        }
    })
        .then(res => {
            if (!res.ok) throw new Error("Network response was not ok");
            return res.json();
        })
        .then(data => {
            if (data.success) {
                Toastify({
                    text: data.message,
                    duration: 3000,
                    style: { background: "green" }
                }).showToast();

                $(`#brand-row-${id}`).remove(); // remove from DOM if needed
            } else {
                throw new Error(data.message || "Delete failed");
            }
        })
        .catch(err => {
            Toastify({
                text: "Error: " + err.message,
                duration: 3000,
                style: { background: "red" }
            }).showToast();
        });
}
