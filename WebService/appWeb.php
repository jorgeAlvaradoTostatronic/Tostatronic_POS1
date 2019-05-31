<?php
	$enlace = mysqli_connect("mysql.tostratonic.com", "tostatronic_symf", "Jorge1995", "tostatronic_symfony");

	if (!$enlace) {
	    echo "Error: No se pudo conectar a MySQL." . PHP_EOL;
	    echo "errno de depuración: " . mysqli_connect_errno() . PHP_EOL;
	    echo "error de depuración: " . mysqli_connect_error() . PHP_EOL;
	    exit;
	}
	$data = file_get_contents("php://input",true);
	$data=json_decode($data);
	foreach ($data as $product) 
	{
		$name = $product->Nombre;
		$code = $product->Codigo;
		$quantity = $product->Cantidad;
		$price = $product->PrecioPublico;
		$image = $product->Imagen;
		$consulta2 = "INSERT INTO `item` (`id`, `item_unit_id`, `item_category_id`, `name`, `code`, `quantity`, `price`, `description`, `active`, `image_description`, `image`, `created`, `updated`) VALUES (NULL, 1, NULL, '".$name."', '".$code."', ".$quantity.", ".$price.", NULL, 1, NULL, '".$image."', NOW(), NULL);";
		if($enlace->query($consulta2)==TRUE)
			echo "Si<br>";
		else
			echo "NO"."<br>".$enlace->error;
	}	
    /* liberar el conjunto de resultados */
    echo "Proceso realizado";
    $resultado->close();
	mysqli_close($enlace);
?>