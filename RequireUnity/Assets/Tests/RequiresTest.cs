using UnityEngine;
using System.Collections;
using Shouldly;
using Require;

public class RequiresTest : TestBehaviour
{
    Transform module;
    Transform sibling;
    Renderer[] renderers;

    public override void Spec()
    {
        Given("it has a parent").And("its parent is a module root").When("it gets the module root").Then("it should return its parent");
        Given("it has a parent").When("it gets the module root").Then("it should return itself");
        Given("it has a parent").And("it is a module root").When("it gets the module root").Then("it should return itself");

        Given("it has a parent").And("its parent is a module root").And("it has a sibling with a renderer").And("its parent has a renderer").When("it gets all renderers in module").Then("it should find 2 renderers");
        Given("it has a parent").And("its parent is a module root").And("it has a sibling with a renderer").And("its sibling is a module root").And("its parent has a renderer").When("it gets all renderers in module").Then("it should find 1 renderers");
    }

    public void ItHasAParent()
    {
        GameObject parent = new GameObject("Parent");
        transform.parent = parent.transform;
    }

    public void ItsParentIsAModuleRoot()
    {
        transform.parent.gameObject.AddComponent<ModuleRoot>();
    }

    public void ItIsAModuleRoot()
    {
        gameObject.AddComponent<ModuleRoot>();
    }

    public void ItHasASiblingWithARenderer()
    {
        sibling = new GameObject("Sibling").AddComponent<MeshRenderer>().transform;
        sibling.parent = transform.parent;
    }

    public void ItsSiblingIsAModuleRoot()
    {
        sibling.gameObject.AddComponent<ModuleRoot>();
    }

    public void ItsParentHasARenderer()
    {
        transform.parent.gameObject.AddComponent<MeshRenderer>();
    }

    public void ItGetsAllRenderersInModule()
    {
        module = transform.GetModuleRoot();
        renderers = module.GetComponentsInModule<Renderer>();
    }

    public void ItGetsTheModuleRoot()
    {
        module = transform.GetModuleRoot();
    }

    public void ItShouldReturnItsParent()
    {
        module.ShouldBe(transform.parent);
    }

    public void ItShouldReturnItself()
    {
        module.ShouldBe(transform);
    }

    public void ItShouldFind__Renderers(int num)
    {
        renderers.Length.ShouldBe(num);
    }

}
