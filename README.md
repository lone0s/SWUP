# Projet_Unity

Projet universitaire de création de widgets complexes, indépendants et intelligents. 

#	Widget Menu : 

- Ajouter le prefab “Menus_panel”
- Récupérer le script attaché au prefab
- Donner le dictionnaire de données au script
- Appeler la méthode d’initialisation
- Possibilité de lui fournir une fonction qui sera appelée lors d’un clic sur un des éléments. La fonction doit avoir en paramètre - un objet UsableObject qui lui sera fourni, étant l’objet associé.
	
#   Widget des Attributs : 

- Ajouter le prefab “Attribut_Panel”
- Récupérer le script dudit prefab
- Lancer la fonction initPanel en passant en paramètre l’instance de l’objet, de type object, dont le panel devra charger les attributs.
- Possibilité de lui fournir une fonction, prenant et retournant un élément de type object, qui sera appelée après la fin de la modification d’un des Input Fields générés à l’exécution.
- Dans le cas de la présence d’un attribut Material dans l’objet cible, attention à bien avoir le prefab OpenFileDialog dans le dossier “Assets/Resources/Prefabs/” pour permettre le changement de texture (seuls les .mat dans le dossier “Assets/Ressources/Materials/”sont tolérés).
    
#    Caméra : 

- Ajoutez le script “Camera_script” à la caméra principale de la scène. 
- Pour suivre un objet il faut appeler la méthode “MoveToTarget” avec en paramètre le GameObject à suivre.

#	  Widget Configuration Caméra :

- Pour ajouter une fonction à exécuter lorsqu'il y a un clic sur le bouton “Reset”, vous pouvez utiliser la méthode “setResetFun” avec en paramètre une fonction sans argument et sans retour.
- Pour que le widget fonctionne, il faut impérativement que le script “Camera_script” soit un composant de la caméra principale.

#    Widget de Création :

- Le préfabriqué “Onglet” et “OpenFileDialog” doivent être dans le dossier “Assets/Ressources/Prefabs/”
- Le projet doit contenir le fichier de classe “UsableObject” qui se trouve dans notre cas dans le dossier “Assets/DataClasses/”
- Pour générer les événements de clic sur l’onglet et sur la croix de l’onglet, il suffit de passer une fonction de type Action<GameObject> aux méthodes respectives “setFunOnCLick” et “setFunOnClickDelete” du script “Add_script”, un composant du bouton “Add”.

#	Widget Explorateur de Fichier :

- Attention a bien avoir le prefab ImagedElementWithAttributes dans le repertoire “Assets/Resources/Prefabs/” 
Deux cas d’utilisation pour ce widget : 
- Si rajouté directement sur la scène : 
    * Renseigner les chemin d'accès pour les icônes et celui sur lequel il démarrera directement dans l’inspecteur et modifier dans le script les attributs triggerFileFilter, triggerLockOnBaseDirectory, triggerMetaFileFilter pour activer/désactiver respectivement le filtre sur les formats de fichiers, le blocage des accès à la hiérarchie des fichiers, et le filtre des fichiers .meta
    * Modifier l’attribut baseDirLockPath pour définir le chemin d'accès bloquant, et modifier l’attribut fileFilterFormat pour définir le format filtré
- Si instancié dynamiquement à partir d’un autre script : 
    * Penser à l’instancier de préférence dans le canvas, de manière à prendre tout l’espace sur l’écran
    * Récupérer le script OpenFileDialog_Script du GameObject instancié
    * Exécuter la fonction updateOFD en lui fournissant respectivement le chemin d'accès sur lequel il réalisera sa première exécution, le format devant être filtré, et le chemin d'accès bloquant. Ces paramètres sont tous optionnels, vous pouvez exécuter updateOFD sans paramètres, le chemin d'accès par défaut et celui du répertoire “Resource” du projet.

