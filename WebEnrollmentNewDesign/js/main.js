

function onSuccess(imageURI) 
{
    
    var pic1 = document.getElementById("photoone");
   
        var changebutton = document.getElementById("cameraid");    

    
        pic1.src = imageURI;
    


      
}

function onFail(message) {
   console.log("Picture failure: " + message);
}
//Camera button functionality
function takepicture()
{

    navigator.camera.getPicture(onSuccess, onFail, { quality: 50, destinationType: Camera.DestinationType.FILE_URI, saveToPhotoAlbum: true });

} 

