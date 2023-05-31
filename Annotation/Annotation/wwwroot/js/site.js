var canvas = document.getElementById("imageCanvas");
var context = canvas.getContext("2d");
var img = new Image();
var annotations = [];

function generateClassId() {

    return Math.floor(Math.random() * 1000);
}
$(document).ready(function () {


    var startX, startY;
    var isDrawing = false;

    // Resim yükleme işlemi
    $("#imageUpload").on("change", function (event) {
        var file = event.target.files[0];
        var reader = new FileReader();

        reader.onload = function (e) {
            img.onload = function () {
                canvas.width = img.width;
                canvas.height = img.height;
                context.drawImage(img, 0, 0);
            }
            img.src = e.target.result;
        }
        reader.readAsDataURL(file);
    });

    // Sınıf listesini getirme işlemi
    $("#getClassesBtn").on("click", function () {
        $.ajax({
            url: "/Home/GetClasses",
            type: "GET",
            success: function (response) {
                $("#classesContainer").html(response);
            },
            error: function () {
                alert("Sınıf listesi alınamadı.");
            }
        });
    });

    // Etiketlemeyi başlatma işlemi
    $("#startAnnotationBtn").on("click", function () {
        //annotations = []; // Önceki etiketleri temizle
        canvas.addEventListener("mousedown", startDrawing);
        canvas.addEventListener("mousemove", drawBoundingBox);
        canvas.addEventListener("mouseup", stopDrawing);
    });

    function startDrawing(e) {
        startX = e.offsetX;
        startY = e.offsetY;
        isDrawing = true;
    }

    function drawBoundingBox(e) {
        if (!isDrawing) return;

        var currentX = e.offsetX;
        var currentY = e.offsetY;
        var width = currentX - startX;
        var height = currentY - startY;

        context.clearRect(0, 0, canvas.width, canvas.height);
        context.drawImage(img, 0, 0);
        drawBox(startX, startY, width, height);
    }

    function saveAnnotations() {
        if (annotations.length === 0) {
            alert("Etiket yok.");
            return;
        }

        // ClassId'yi otomatik olarak atama
        var classId = generateClassId();

        for (var i = 0; i < annotations.length; i++) {
            annotations[i].ClassId = classId;
        }

        $.ajax({
            url: "/Home/SaveAnnotations",
            type: "POST",
            data: JSON.stringify({ annotations: annotations }),
            contentType: "application/json",
            success: function () {
                alert("Etiketler kaydedildi.");
                context.clearRect(0, 0, canvas.width, canvas.height);
                context.drawImage(img, 0, 0);
                console.log(JSON.stringify({ annotations: annotations }));
            },
            error: function () {
                alert("Etiketler kaydedilemedi.");
            }
        });
    }

    function stopDrawing(e) {
        isDrawing = false;

        var currentX = e.offsetX;
        var currentY = e.offsetY;
        var width = currentX - startX;
        var height = currentY - startY;

        var className = $("#ClassName").val();

        if (width !== 0 && height !== 0) {
            if (className.trim() === '') {
                alert("Sınıf adını giriniz.");
                return;
            }

            var annotation = {
                X: startX,
                Y: startY,
                Width: width,
                Height: height,
                ClassName: className
            };
            annotations.push(annotation);
        }
    }

    function drawBox(x, y, width, height) {
        context.beginPath();
        context.rect(x, y, width, height);
        context.lineWidth = 2;
        context.strokeStyle = "red";
        context.fillStyle = "transparent";
        context.stroke();
    }

    // Annotation.txt dosyasını getirme işlemi
    $("#getAnnotationsBtn").on("click", function () {
        $.ajax({
            url: "/Home/GetAnnotations",
            type: "GET",
            success: function (response) {
                $("#annotationsContainer").html(response);
            },
            error: function () {
                alert("Annotation.txt alınamadı.");
            }
        });
    });

    // Etiketleri kaydetme işlemi
    $("#saveAnnotationsBtn").on("click", function () {
        saveAnnotations();
    });
});
