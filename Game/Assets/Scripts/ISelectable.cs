/*
 * Interface to determine if an object can be selected
 * Interfaces are kinda confusing
 * Good explanation: https://www.w3schools.com/cs/cs_interface.php
 */
public interface ISelectable {
  public bool IsHovered { get; set; }
  public bool IsSelected { get; set; }

}
